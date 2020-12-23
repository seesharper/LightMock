using System;
using System.Linq;
using Xunit;

namespace LightMock.Tests
{
    public class MockContextTests
    {
        [Fact]
        public void Assert_Never_IsVerified()
        {
            var mockContext = new MockContext<IFoo>();
            mockContext.Assert(f => f.Execute("SomeValue"), Invoked.Never);
        }

        [Fact]
        public void Assert_NeverWhenInvoked_ThrowsException()
        {
            var mockContext = new MockContext<IFoo>();
            var fooMock = new FooMock(mockContext);
            fooMock.Execute("SomeValue");
            Assert.Throws<InvalidOperationException>(() => mockContext.Assert(f => f.Execute("SomeValue"), Invoked.Never));
        }

        [Fact]
        public void Assert_Once_IsVerified()
        {
            var mockContext = new MockContext<IFoo>();
            var fooMock = new FooMock(mockContext);
            fooMock.Execute("SomeValue");
            mockContext.Assert(f => f.Execute("SomeValue"));
        }

        [Fact]
        public void Assert_Twice_IsVerified()
        {
            var mockContext = new MockContext<IFoo>();
            var fooMock = new FooMock(mockContext);
            fooMock.Execute("SomeValue");
            fooMock.Execute("SomeValue");
            mockContext.Assert(f => f.Execute("SomeValue"));
        }

        [Fact]
        public void Assert_WithoutInvocation_ThrowsException()
        {
            var mockContext = new MockContext<IFoo>();
            Assert.Throws<InvalidOperationException>(() => mockContext.Assert(f => f.Execute("SomeValue")));
        }

        [Fact]
        public void Assert_ExpectedOnceWithoutInvocation_ThrowsException()
        {
            var mockContext = new MockContext<IFoo>();
            Assert.Throws<InvalidOperationException>(() => mockContext.Assert(f => f.Execute("SomeValue"), Invoked.Once));
        }

        [Fact]
        public void Assert_InvokedOnceExpectedOnce_IsVerified()
        {
            var mockContext = new MockContext<IFoo>();
            var fooMock = new FooMock(mockContext);
            fooMock.Execute("SomeValue");
            mockContext.Assert(f => f.Execute("SomeValue"), Invoked.Once);
        }

        [Fact]
        public void Assert_InvokedTwiceWithExpectedOnce_ThrowsException()
        {
            var mockContext = new MockContext<IFoo>();
            var fooMock = new FooMock(mockContext);
            fooMock.Execute("SomeValue");
            fooMock.Execute("SomeValue");
            Assert.Throws<InvalidOperationException>(() => mockContext.Assert(f => f.Execute("SomeValue"), Invoked.Once));
        }

        [Fact]
        public void Assert_WithValidMatchPredicate_IsVerified()
        {
            var mockContext = new MockContext<IFoo>();
            var fooMock = new FooMock(mockContext);
            fooMock.Execute("SomeValue");
            mockContext.Assert(f => f.Execute(The<string>.Is(s => s.StartsWith("Some"))), Invoked.Once);
        }

        [Fact]
        public void Assert_WithInvalidMatchPredicate_ThrowsException()
        {
            var mockContext = new MockContext<IFoo>();
            var fooMock = new FooMock(mockContext);
            fooMock.Execute("SomeValue");
            Assert.Throws<InvalidOperationException>(()
                => mockContext.Assert(f => f.Execute(The<string>.Is(s => s == "AnotherValue")), Invoked.Once));
        }

        [Fact]
        public void Assert_ExpectedAtLeast3TimesAnd2TimesInvoked_ThrowsException()
        {
            var mockContext = new MockContext<IFoo>();
            var fooMock = new FooMock(mockContext);
            fooMock.Execute("SomeValue");
            fooMock.Execute("SomeValue");
            Assert.Throws<InvalidOperationException>(() => mockContext.Assert(f => f.Execute("SomeValue"), Invoked.AtLeast(3)));
        }

        [Fact]
        public void Assert_ExpectedAtLeast3TimesAnd3TimesInvoked_IsVerified()
        {
            var mockContext = new MockContext<IFoo>();
            var fooMock = new FooMock(mockContext);
            fooMock.Execute("SomeValue");
            fooMock.Execute("SomeValue");
            fooMock.Execute("SomeValue");
            mockContext.Assert(f => f.Execute("SomeValue"), Invoked.AtLeast(3));
        }

        [Fact]
        public void Assert_ExpectedAtLeast3TimesAnd4TimesInvoked_IsVerified()
        {
            var mockContext = new MockContext<IFoo>();
            var fooMock = new FooMock(mockContext);
            fooMock.Execute("SomeValue");
            fooMock.Execute("SomeValue");
            fooMock.Execute("SomeValue");
            fooMock.Execute("SomeValue");
            mockContext.Assert(f => f.Execute("SomeValue"), Invoked.AtLeast(3));
        }

        [Fact]
        public void Assert_ExpectedExactly3TimesAnd2TimesInvoked_ThrowsException()
        {
            var mockContext = new MockContext<IFoo>();
            var fooMock = new FooMock(mockContext);
            fooMock.Execute("SomeValue");
            fooMock.Execute("SomeValue");
            Assert.Throws<InvalidOperationException>(() => mockContext.Assert(f => f.Execute("SomeValue"), Invoked.Exactly(3)));
        }

        [Fact]
        public void Assert_ExpectedExactly3TimesAnd3TimesInvoked_IsVerified()
        {
            var mockContext = new MockContext<IFoo>();
            var fooMock = new FooMock(mockContext);
            fooMock.Execute("SomeValue");
            fooMock.Execute("SomeValue");
            fooMock.Execute("SomeValue");
            mockContext.Assert(f => f.Execute("SomeValue"), Invoked.Exactly(3));
        }

        [Fact]
        public void Assert_ExpectedExactly3TimesAnd4TimesInvoked_ThrowsException()
        {
            var mockContext = new MockContext<IFoo>();
            var fooMock = new FooMock(mockContext);
            fooMock.Execute("SomeValue");
            fooMock.Execute("SomeValue");
            fooMock.Execute("SomeValue");
            fooMock.Execute("SomeValue");
            Assert.Throws<InvalidOperationException>(() => mockContext.Assert(f => f.Execute("SomeValue"), Invoked.Exactly(3)));
        }

        [Fact]
        public void Assert_IsAnyValue_IsVerified()
        {
            var mockContext = new MockContext<IFoo>();
            var fooMock = new FooMock(mockContext);
            fooMock.Execute("SomeValue");
            mockContext.Assert(f => f.Execute(The<string>.IsAnyValue), Invoked.Once);
        }

        [Fact]
        public void Arrange_Exception_ThrowsException()
        {
            var mockContext = new MockContext<IFoo>();
            var fooMock = new FooMock(mockContext);
            mockContext.Arrange(f => f.Execute("SomeValue")).Throws<InvalidOperationException>();
            Assert.Throws<InvalidOperationException>(() => fooMock.Execute("SomeValue"));
        }

        [Fact]
        public void Arrange_ExceptionFactory_ThrowsException()
        {
            var mockContext = new MockContext<IFoo>();
            var fooMock = new FooMock(mockContext);
            mockContext.Arrange(f => f.Execute("SomeValue")).Throws(() => new InvalidOperationException());
            Assert.Throws<InvalidOperationException>(() => fooMock.Execute("SomeValue"));
        }



        [Fact]
        public void Execute_ArrengedReturnValue_ReturnsValue()
        {
            var mockContext = new MockContext<IFoo>();
            var fooMock = new FooMock(mockContext);
            mockContext.Arrange(f => f.Execute()).Returns("SomeValue");

            var result = fooMock.Execute();

            Assert.Equal("SomeValue", result);
        }

        [Fact]
        public void Execute_NoArrangement_ReturnsDefaultValue()
        {
            var mockContext = new MockContext<IFoo>();
            var fooMock = new FooMock(mockContext);

            string result = fooMock.Execute();

            Assert.Equal(default(string), result);
        }

        [Fact]
        public void Execute_MethodCallInInvocation_IsVerified()
        {
            var mockContext = new MockContext<IFoo>();
            var fooMock = new FooMock(mockContext);

            string[] strings = { "SomeValue", "AnotherValue" };

            fooMock.Execute(strings.First(s => s.StartsWith("Some")));
        }

        [Fact]
        public void Assert_Null_IsVerified()
        {
            var mockContext = new MockContext<IFoo>();
            var fooMock = new FooMock(mockContext);

            fooMock.Execute((string)null);

            mockContext.Assert(f => f.Execute((string)null));
        }

        [Fact]
        public void Assert_StringEmpty_IsVerified()
        {
            var mockContext = new MockContext<IFoo>();
            var fooMock = new FooMock(mockContext);

            fooMock.Execute(string.Empty);

            mockContext.Assert(f => f.Execute(string.Empty));
        }

        [Fact]
        public void Execute_ArrangedReturnValue_ReturnsValueUsingLambda()
        {
            var mockContext = new MockContext<IFoo>();
            var fooMock = new FooMock(mockContext);
            var outerVariable = " works";
            mockContext.Arrange(f => f.Execute(The<string>.IsAnyValue)).Returns<string>(
                a =>
                {
                    var r = a + outerVariable + "!";
                    return r;
                });

            var result = fooMock.Execute("It");

            Assert.Equal("It works!", result);
        }

        [Fact]
        public void Execute_ArrangedRuturnValue_ArrayLengthContraintSupport()
        {
            var mockContext = new MockContext<IFoo>();
            var fooMock = new FooMock(mockContext);
            var inputData = Enumerable.Repeat((byte)1, 10).ToArray();
            mockContext.Arrange(
                f => f.Execute(The<byte[]>.Is(a => a.Length < 20)))
                .Returns<byte[]>(a => a);
            var result = fooMock.Execute(inputData);
            Assert.Equal(inputData, result);
        }

        [Fact]
        public void Execute_ArrangedRuturnValue_ArrayElementContraintSupport()
        {
            var mockContext = new MockContext<IFoo>();
            var fooMock = new FooMock(mockContext);
            var inputData = new byte[] { 1, 2, 3, 4, 5 };
            mockContext.Arrange(
                f => f.Execute(The<byte[]>.Is(a => a[4] == 5)))
                .Returns<byte[]>(a => a);
            var result = fooMock.Execute(inputData);
            Assert.Equal(inputData, result);
        }

        [Fact]
        public void PropertySupport_ArrangeGetterReturnValue_IsVerified()
        {
            var mockContext = new MockContext<IProp>();
            var propMock = new PropMock(mockContext);
            mockContext.Arrange(x => x.ReadOnlyProperty).Returns(() => "result");

            var propertyValue = propMock.ReadOnlyProperty;
            Assert.Equal("result", propertyValue);
        }

        [Fact]
        public void PropertySupport_ArrangeGetterAndSetter_IsVerified()
        {
            var mockContext = new MockContext<IProp>();
            var propMock = new PropMock(mockContext);
            mockContext.ArrangeProperty(x => x.TwoWayProperty);

            var propertyValue = propMock.TwoWayProperty;
            Assert.Null(propertyValue);

            propMock.TwoWayProperty = "customValue";

            propertyValue = propMock.TwoWayProperty;
            Assert.Equal("customValue", propertyValue);
        }
    }
}