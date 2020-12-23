using Xunit;

namespace LightMock.Tests
{
    public class ArrangementTests
    {
        [Fact]
        public void Arrange_CallBackNoArguments_InvokesCallback()
        {
            var mockContext = new MockContext<IFoo>();
            var fooMock = new FooMock(mockContext);
            bool isCalled = false;
            mockContext.Arrange(f => f.Execute()).Callback(() => isCalled = true);
            fooMock.Execute();
            Assert.True(isCalled);
        }

        [Fact]
        public void Arrange_CallBackOneArgument_InvokesCallback()
        {
            var mockContext = new MockContext<IFoo>();
            var fooMock = new FooMock(mockContext);
            int callBackResult = 0;
            mockContext.Arrange(f => f.Execute(The<int>.IsAnyValue)).Callback<int>(s => callBackResult = s);
            fooMock.Execute(1);
            Assert.Equal(1, callBackResult);
        }

        [Fact]
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
            Assert.Equal(1, firstResult);
            Assert.Equal(2, secondResult);
        }

        [Fact]
        public void Arrange_CallBackThreeArguments_InvokesCallback()
        {
            var mockContext = new MockContext<IFoo>();
            var fooMock = new FooMock(mockContext);
            int firstResult = 0;
            int secondResult = 0;
            int thirdResult = 0;
            mockContext.Arrange(f => f.Execute(The<int>.IsAnyValue, The<int>.IsAnyValue, The<int>.IsAnyValue))
                .Callback<int, int, int>(
                    (i1, i2, i3) =>
                    {
                        firstResult = i1;
                        secondResult = i2;
                        thirdResult = i3;
                    });

            fooMock.Execute(1, 2, 3);

            Assert.Equal(1, firstResult);
            Assert.Equal(2, secondResult);
            Assert.Equal(3, thirdResult);
        }

        [Fact]
        public void Arrange_CallBackFourArguments_InvokesCallback()
        {
            var mockContext = new MockContext<IFoo>();
            var fooMock = new FooMock(mockContext);
            int firstResult = 0;
            int secondResult = 0;
            int thirdResult = 0;
            int fourthResult = 0;
            mockContext.Arrange(
                f => f.Execute(The<int>.IsAnyValue, The<int>.IsAnyValue, The<int>.IsAnyValue, The<int>.IsAnyValue))
                .Callback<int, int, int, int>(
                    (i1, i2, i3, i4) =>
                    {
                        firstResult = i1;
                        secondResult = i2;
                        thirdResult = i3;
                        fourthResult = i4;
                    });

            fooMock.Execute(1, 2, 3, 4);

            Assert.Equal(1, firstResult);
            Assert.Equal(2, secondResult);
            Assert.Equal(3, thirdResult);
            Assert.Equal(4, fourthResult);
        }

        [Fact]
        public void Arrange_CallBackFiveArguments_InvokesCallback()
        {
            var mockContext = new MockContext<IFoo>();
            var fooMock = new FooMock(mockContext);
            int firstResult = 0;
            int secondResult = 0;
            int thirdResult = 0;
            int fourthResult = 0;
            int fifthResult = 0;
            mockContext.Arrange(
                f => f.Execute(The<int>.IsAnyValue, The<int>.IsAnyValue, The<int>.IsAnyValue, The<int>.IsAnyValue, The<int>.IsAnyValue))
                .Callback<int, int, int, int, int>(
                    (i1, i2, i3, i4, i5) =>
                    {
                        firstResult = i1;
                        secondResult = i2;
                        thirdResult = i3;
                        fourthResult = i4;
                        fifthResult = i5;
                    });

            fooMock.Execute(1, 2, 3, 4, 5);

            Assert.Equal(1, firstResult);
            Assert.Equal(2, secondResult);
            Assert.Equal(3, thirdResult);
            Assert.Equal(4, fourthResult);
            Assert.Equal(5, fifthResult);
        }

        [Fact]
        public void Arrange_CallBackSixArguments_InvokesCallback()
        {
            var mockContext = new MockContext<IFoo>();
            var fooMock = new FooMock(mockContext);
            int firstResult = 0;
            int secondResult = 0;
            int thirdResult = 0;
            int fourthResult = 0;
            int fifthResult = 0;
            int sixthResult = 0;
            mockContext.Arrange(
                f => f.Execute(The<int>.IsAnyValue, The<int>.IsAnyValue, The<int>.IsAnyValue, The<int>.IsAnyValue, The<int>.IsAnyValue, The<int>.IsAnyValue))
                .Callback<int, int, int, int, int, int>(
                    (i1, i2, i3, i4, i5, i6) =>
                    {
                        firstResult = i1;
                        secondResult = i2;
                        thirdResult = i3;
                        fourthResult = i4;
                        fifthResult = i5;
                        sixthResult = i6;
                    });

            fooMock.Execute(1, 2, 3, 4, 5, 6);

            Assert.Equal(1, firstResult);
            Assert.Equal(2, secondResult);
            Assert.Equal(3, thirdResult);
            Assert.Equal(4, fourthResult);
            Assert.Equal(5, fifthResult);
            Assert.Equal(6, sixthResult);
        }

        [Fact]
        public void Arrange_ReturnsWithNoArguments_InvokesGetResult()
        {
            var mockContext = new MockContext<IFoo>();
            var fooMock = new FooMock(mockContext);

            mockContext.Arrange(f => f.Execute()).Returns(() => "This");
            var result = fooMock.Execute();
            Assert.Equal("This", result);
        }

        [Fact]
        public void Arrange_ReturnsWithOneArgument_InvokesGetResult()
        {
            var mockContext = new MockContext<IFoo>();
            var fooMock = new FooMock(mockContext);
            mockContext.Arrange(f => f.Execute(The<string>.IsAnyValue)).Returns<string>(a => "This" + a);
            var result = fooMock.Execute(" is");
            Assert.Equal("This is", result);
        }

        [Fact]
        public void Arrange_ReturnsWithTwoArguments_InvokesGetResult()
        {
            var mockContext = new MockContext<IFoo>();
            var fooMock = new FooMock(mockContext);
            mockContext.Arrange(f => f.Execute(The<string>.IsAnyValue, The<string>.IsAnyValue))
                .Returns<string, string>((a, b) => "This" + a + b);
            var result = fooMock.Execute(" is", " really");
            Assert.Equal("This is really", result);
        }

        [Fact]
        public void Arrange_ReturnsWithThreeArguments_InvokesGetResult()
        {
            var mockContext = new MockContext<IFoo>();
            var fooMock = new FooMock(mockContext);
            mockContext.Arrange(f => f.Execute(The<string>.IsAnyValue, The<string>.IsAnyValue, The<string>.IsAnyValue))
                .Returns<string, string, string>((a, b, c) => "This" + a + b + c);

            var result = fooMock.Execute(" is", " really", " cool");
            Assert.Equal("This is really cool", result);
        }

        [Fact]
        public void Arrange_ReturnsWithFourArguments_InvokesGetResult()
        {
            var mockContext = new MockContext<IFoo>();
            var fooMock = new FooMock(mockContext);
            mockContext.Arrange(
                f =>
                    f.Execute(
                        The<string>.IsAnyValue,
                        The<string>.IsAnyValue,
                        The<string>.IsAnyValue,
                        The<string>.IsAnyValue))
                .Returns<string, string, string, string>((a, b, c, d) => "This" + a + b + c + d);
            fooMock.Execute(1, 2, 3, 4);

            var result = fooMock.Execute(" is", " really", " cool", "!");
            Assert.Equal("This is really cool!", result);
        }
    }
}