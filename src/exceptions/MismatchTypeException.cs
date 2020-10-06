namespace ivy.serialization.inline.exceptions
{
    using System;

    public class MismatchTypeException : Exception
    {
        public MismatchTypeException(string msg) : base(msg)  { }
    }
}