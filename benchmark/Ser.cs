namespace benchmark
{
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Running;
    using ivy.serialization.inline;
    using Newtonsoft.Json;

    [SimpleJob]
    public class Ser
    {
        private BinaryFormatter bin;
        [GlobalSetup]
        public void Setup()
        {
            bin = new BinaryFormatter();
        }

        [Benchmark]
        public void InlineSer()
        {
            FastSerializer.Serialize(12f);
            FastSerializer.Serialize("test-string");
            FastSerializer.Serialize(true);
            FastSerializer.Serialize(ulong.MaxValue);
        }
        [Benchmark]
        public void Bin()
        {
            BinSer(12f);
            BinSer("test-string");
            BinSer(true);
            BinSer(ulong.MaxValue);
        }

        [Benchmark]
        public void Json()
        {
            JsonConvert.SerializeObject(12f);
            JsonConvert.SerializeObject("test-string");
            JsonConvert.SerializeObject(true);
            JsonConvert.SerializeObject(ulong.MaxValue);
        }


        private void BinSer<T>(T t)
        {
            using var stream = new MemoryStream();
            bin.Serialize(stream, t);
        }
    }

    public class Program
    {
        public static void Main(string[] args) => BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
    }
}