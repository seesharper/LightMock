using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LightMock.Tests
{
    using System.Linq;

    using LightMock;

    [TestClass]
    public class MoqContextTests
    {
        [TestMethod]
        public void Verify_Once_IsVerified()
        {
            var mockContext = new MockContext<IFoo>();
            var fooMock = new FooMock(mockContext);            
            fooMock.Execute("SomeValue");            
            mockContext.Assert(f => f.Execute("SomeValue"));
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void Verify_Twice_IsVerified()
        {
            var mockContext = new MockContext<IFoo>();
            var fooMock = new FooMock(mockContext);
            fooMock.Execute("SomeValue");
            fooMock.Execute("SomeValue");
            mockContext.Assert(f => f.Execute("SomeValue"));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Execute_ZeroTimes_ThrowsException()
        {
            var mockContext = new MockContext<IFoo>();            
            mockContext.Assert(f => f.Execute("SomeValue"));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Execute_ZeroTimesWithExpectedOnce_ThrowsException()
        {
            var mockContext = new MockContext<IFoo>();
            mockContext.Assert(f => f.Execute("SomeValue"), Invoked.Once);
        }

        [TestMethod]
        public void Execute_OneTimesWithExpectedOnce_IsVerified()
        {
            var mockContext = new MockContext<IFoo>();
            var fooMock = new FooMock(mockContext);
            fooMock.Execute("SomeValue");
            mockContext.Assert(f => f.Execute("SomeValue"), Invoked.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Execute_TwiceWithExpectedOnce_ThrowsException()
        {
            var mockContext = new MockContext<IFoo>();
            var fooMock = new FooMock(mockContext);
            fooMock.Execute("SomeValue");
            fooMock.Execute("SomeValue");
            mockContext.Assert(f => f.Execute("SomeValue"), Invoked.Once);
        }

        [TestMethod]
        public void Execute_WithValidMatchPredicate_IsVerified()
        {
            var mockContext = new MockContext<IFoo>();
            var fooMock = new FooMock(mockContext);
            fooMock.Execute("SomeValue");                        
            mockContext.Assert(f => f.Execute(The<string>.Is(s => s.StartsWith("Some"))), Invoked.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Execute_WithInvalidMatchPredicate_ThrowsException()
        {
            var mockContext = new MockContext<IFoo>();
            var fooMock = new FooMock(mockContext);
            fooMock.Execute("SomeValue");
            mockContext.Assert(f => f.Execute(The<string>.Is(s => s == "AnotherValue")), Invoked.Once);
        }

        [TestMethod]
        public void Execute_IsAnyValue_IsVerified()
        {
            var mockContext = new MockContext<IFoo>();
            var fooMock = new FooMock(mockContext);
            fooMock.Execute("SomeValue");
            mockContext.Assert(f => f.Execute(The<string>.IsAnyValue), Invoked.Once);                        
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Arrange_Exception_ThrowsException()
        {
            var mockContext = new MockContext<IFoo>();
            var fooMock = new FooMock(mockContext);
            mockContext.Arrange(f => f.Execute("SomeValue")).Throws<InvalidOperationException>();
            fooMock.Execute("SomeValue");
        }

        [TestMethod]
        public void Arrange_CallBack_InvokesCallback()
        {
            var mockContext = new MockContext<IFoo>();
            var fooMock = new FooMock(mockContext);
            string callBackResult = null;
            mockContext.Arrange(f => f.Execute(The<string>.IsAnyValue)).Callback<string>(s => callBackResult = s);
            fooMock.Execute("SomeValue");
            Assert.AreEqual("SomeValue", callBackResult);
        }

        [TestMethod]
        public void Execute_ArrengedReturnValue_ReturnsValue()
        {
            var mockContext = new MockContext<IFoo>();
            var fooMock = new FooMock(mockContext);
            mockContext.Arrange(f => f.Execute()).Returns("SomeValue");

            var result = fooMock.Execute();

            Assert.AreEqual("SomeValue", result);
        }

        [TestMethod]
        public void Execute_NoArrangement_ReturnsDefaultValue()
        {
            var mockContext = new MockContext<IFoo>();
            var fooMock = new FooMock(mockContext);            

            string result = fooMock.Execute();

            Assert.AreEqual(default(string), result);
        }


        [TestMethod]
        public void Execute_MethodCallInInvocation_IsVerified()
        {
            var mockContext = new MockContext<IFoo>();
            var fooMock = new FooMock(mockContext);

            string[] strings = new[] { "SomeValue", "AnotherValue" };

            fooMock.Execute(strings.First(s => s.StartsWith("Some")));
        }


    }
}
