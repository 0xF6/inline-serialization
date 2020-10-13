namespace test
{
    using ivy.serialization.inline;
    using Xunit;

    public class Base
    {
        [Fact]
        public void Test00()
        {
            var s = FastSerializer.Serialize("test-string");
            Assert.Equal("s00test-string", s);
        }
        [Fact]
        public void Test01()
        {
            var s = FastSerializer.UnSerialize<string>("s00test-string");
            Assert.Equal("test-string", s);
        }
        [Fact]
        public void Test02()
        {
            var s = FastSerializer.Serialize(12f);
            Assert.Equal("f2012", s);
        }
        [Fact]
        public void Test03()
        {
            var s = FastSerializer.Serialize(true);
            Assert.Equal("i02True", s);
        }
        [Fact]
        public void Test04()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ru");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ru");
            var s = FastSerializer.UnSerialize<float>("f2012.12");
            Assert.Equal(12.12f, s);
        }
    }
}
