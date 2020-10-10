namespace ivy.serialization.inline
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.Serialization;
    using static System.StringComparison;
    using exceptions;

    public static class FastSerializer
    {
        #region mapping

        private static readonly IDictionary<(char, byte), Type> mapping = new Dictionary<(char, byte), Type>
        {
            { ('i', 2  ), typeof(bool)    },
            { ('s', 8  ), typeof(sbyte)   },
            { ('i', 8  ), typeof(byte)    },

            { ('i', 16 ), typeof(short)   },
            { ('u', 16 ), typeof(ushort)  },

            { ('i', 32 ), typeof(int)     },
            { ('u', 32 ), typeof(uint)    },

            { ('i', 64 ), typeof(long)    },
            { ('u', 64 ), typeof(ulong)   },
            { ('f', 32 ), typeof(float)   },
            { ('f', 64 ), typeof(double)  },
            { ('f', 128), typeof(decimal) },

            { ('s', 00 ), typeof(string)   }
        };
        private static readonly IDictionary<Type, (char, byte)> mapping_rv = new Dictionary<Type, (char, byte)>
        {
            { typeof(bool),   ('i', 2  )    },
            { typeof(sbyte),  ('s', 8  )    },
            { typeof(byte),   ('i', 8  )    },

            { typeof(short),  ('i', 16 )    },
            { typeof(ushort), ('u', 16 )    },

            { typeof(int),    ('i', 32 )    },
            { typeof(uint),   ('u', 32 )    },

            { typeof(long),   ('i', 64 )    },
            { typeof(ulong),  ('u', 64 )    },
            { typeof(float),  ('f', 32 )    },
            { typeof(double), ('f', 64 )    },
            { typeof(decimal),('f', 128)    },

            { typeof(string), ('s', 00 )    }
        };

        private static readonly IDictionary<Type, Delegate> cacheBinder = new Dictionary<Type, Delegate>();

        #endregion


        public static string Serialize<T>(T type)
        {
            if (!mapping_rv.ContainsKey(typeof(T)))
                throw new SerializationException($"Fast serialization not support '{typeof(T)}'.");
            var (s, o) = mapping_rv[typeof(T)];
            // maybe need using another way for cast to string with invariant culture
            return $"{s}{o:X2}{string.Format(CultureInfo.InvariantCulture, "{0}", type)}";
        }
        
        public static T UnSerialize<T>(string s)
        {
            if (string.IsNullOrEmpty(s))
                return default;

            if (s.Length < 3)
                throw new SerializationException($"Invalid input string.");

            var c = GetConverter<T>();
            var header = Take(s, 3);
            var body = Skip(s, 3);
            
            if (!isMappedSymbol(header[0]))
                throw new SerializationException($"Invalid header format. [{header}]");

            if (isNotHex(header[1]) && isNotHex(header[2]))
                throw new SerializationException($"Invalid header format. [{header}]");

            var idx = byte.Parse($"{header[1]}{header[2]}", NumberStyles.HexNumber);


            var key = (header[0], idx);

            if (!mapping.ContainsKey(key))
                throw new SerializationException($"Failed bind converter for '{header}' header.");
            if (mapping[key] != typeof(T))
                throw new MismatchTypeException($"Type '{header}'({mapping[key]}) is not compatible with '{typeof(T)}'.");

            return c(body);
        }
        // TODO rewrite to switch-case variant
        private static Func<string, T> GetConverter<T>()
        {
            if (!mapping_rv.ContainsKey(typeof(T)))
                throw new SerializationException($"Fast serialization not support '{typeof(T)}'.");

            static Func<string, I> bind<I>()
            {
                if (typeof(I) == typeof(string))
                    return z => (I)(object)z;

                if (cacheBinder.ContainsKey(typeof(I)))
                    return cacheBinder[typeof(I)] as Func<string, I>;
                return (Func<string, I>)(cacheBinder[typeof(I)] = typeof(I)
                    .GetMethods()
                    .Where(x => x.IsStatic)
                    .Where(x => x.Name.Equals("parse", InvariantCultureIgnoreCase))
                    .Where(x => x.GetParameters().Length == 1)
                    .First(x => x.GetParameters().First().ParameterType == typeof(string))
                    .CreateDelegate(typeof(Func<string, I>)));
            }
            return bind<T>();
        }


        #region Etc


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool isMappedSymbol(char c) 
            => c == 'i' || c == 's' || c == 'f' || c == 'u';

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool isNotHex(char c)
            => !((c >= '0' && c <= '9') ||
                (c >= 'a' && c <= 'f') ||
                (c >= 'A' && c <= 'F'));

        private static string Take(string s, byte len)
        {
            var target = new char[len];
            Array.Copy(s.ToArray(), target, len);
            return string.Join(string.Empty, target);
        }
        private static string Skip(string s, byte len)
        {
            var tlen = s.Length - len;
            var target = new char[tlen];
            Array.Copy(s.ToCharArray(len, tlen), target, tlen);
            return string.Join(string.Empty, target);
        }

        #endregion
    }
}