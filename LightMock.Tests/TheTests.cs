namespace LightMock.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TheTests
    {
        [TestMethod]
        public void IsAnyValue_ReturnsDefaultValue()
        {
            Assert.AreEqual(default(string), The<string>.IsAnyValue);            
        }

        [TestMethod]
        public void Is_AnyPredicate_ReturnsDefaultValue()
        {
            Assert.AreEqual(default(string), The<string>.Is(s => true));            
        }
    }
}