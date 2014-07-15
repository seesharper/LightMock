namespace LightMock.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ArrangementTests
    {
        [TestMethod]
        public void Arrange_CallBackNoArguments_InvokesCallback()
        {
            var mockContext = new MockContext<IFoo>();
            var fooMock = new FooMock(mockContext);
            bool isCalled = false;
            mockContext.Arrange(f => f.Execute()).Callback(() => isCalled = true);
            fooMock.Execute();
            Assert.IsTrue(isCalled);
        }
                
        [TestMethod]
        public void Arrange_CallBackOneArgument_InvokesCallback()
        {
            var mockContext = new MockContext<IFoo>();
            var fooMock = new FooMock(mockContext);
            int callBackResult = 0;
            mockContext.Arrange(f => f.Execute(The<int>.IsAnyValue)).Callback<int>(s => callBackResult = s);
            fooMock.Execute(1);
            Assert.AreEqual(1, callBackResult);
        }

        [TestMethod]
        public void Arrange_CallBackTwoArguments_InvokesCallback()
        {
            var mockContext = new MockContext<IFoo>();
            var fooMock = new FooMock(mockContext);
            int firstResult = 0;
            int secondResult = 0;
            mockContext.Arrange(f => f.Execute(The<int>.IsAnyValue, The<int>.IsAnyValue)).Callback<int, int>(
                (i, i1) =>
                    {
                        firstResult = i;
                        secondResult = i1;
                    });
            fooMock.Execute(1, 2);
            Assert.AreEqual(1, firstResult);
            Assert.AreEqual(2, secondResult);
        }

        [TestMethod]
        public void Arrange_CallBackThreeArguments_InvokesCallback()
        {
            var mockContext = new MockContext<IFoo>();
            var fooMock = new FooMock(mockContext);
            int firstResult = 0;
            int secondResult = 0;
            int thirdResult = 0;
            mockContext.Arrange(f => f.Execute(The<int>.IsAnyValue, The<int>.IsAnyValue, The<int>.IsAnyValue)).Callback<int, int, int>(
                (i1, i2, i3) =>
                {
                    firstResult = i1;
                    secondResult = i2;
                    thirdResult = i3;
                });
            
            fooMock.Execute(1, 2, 3);
            
            Assert.AreEqual(1, firstResult);
            Assert.AreEqual(2, secondResult);
            Assert.AreEqual(3, thirdResult);
        }

        [TestMethod]
        public void Arrange_CallBackFourArguments_InvokesCallback()
        {
            var mockContext = new MockContext<IFoo>();
            var fooMock = new FooMock(mockContext);
            int firstResult = 0;
            int secondResult = 0;
            int thirdResult = 0;
            int fourthResult = 0;
            mockContext.Arrange(f => f.Execute(The<int>.IsAnyValue, The<int>.IsAnyValue, The<int>.IsAnyValue, The<int>.IsAnyValue)).Callback<int, int, int, int>(
                (i1, i2, i3, i4) =>
                {
                    firstResult = i1;
                    secondResult = i2;
                    thirdResult = i3;
                    fourthResult = i4;
                });

            fooMock.Execute(1, 2, 3, 4);

            Assert.AreEqual(1, firstResult);
            Assert.AreEqual(2, secondResult);
            Assert.AreEqual(3, thirdResult);
            Assert.AreEqual(4, fourthResult);
        }


        
    }
}