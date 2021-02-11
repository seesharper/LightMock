using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Xunit;

namespace LightMock.Tests
{
    public class MatchInfoTests
    {
        [Fact]
        public void Matches_SameValue_ReturnsTrue()
        {
            var predicateBuilder = new MatchInfoBuilder();
            Expression<Action<IFoo>> expression = (f) => f.Execute("SomeValue");

            var matchInfo = predicateBuilder.Build(expression);


            var invocationInfo = new InvocationInfo(
                typeof(IFoo).GetMethod("Execute", new Type[] { typeof(string) }),
                new[] { "SomeValue" });

            Assert.True(matchInfo.Matches(invocationInfo));
        }

        [Fact]
        public void Matches_DifferentValue_ReturnsFalse()
        {
            var predicateBuilder = new MatchInfoBuilder();
            Expression<Action<IFoo>> expression = (f) => f.Execute("SomeValue");

            var matchInfo = predicateBuilder.Build(expression);

            var invocationInfo = new InvocationInfo(
                typeof(IFoo).GetMethod("Execute", new Type[] { typeof(string) }),
                new[] { "AnotherValue" });

            Assert.False(matchInfo.Matches(invocationInfo));
        }

        [Fact]
        public void Matches_SameValueDifferentMethod_ReturnsFalse()
        {
            var predicateBuilder = new MatchInfoBuilder();
            Expression<Action<IFoo>> expression = (f) => f.Execute("SomeValue");

            var matchInfo = predicateBuilder.Build(expression);

            var invocationInfo = new InvocationInfo(
                typeof(IBar).GetMethod("Execute", new Type[] { typeof(string) }),
                new[] { "SomeValue" });

            Assert.False(matchInfo.Matches(invocationInfo));
        }

        [Fact]
        public void Matches_ArgumentCountMismatch_ReturnsFalse()
        {
            var predicateBuilder = new MatchInfoBuilder();
            Expression<Action<IFoo>> expression = (f) => f.Execute("SomeValue");

            var matchInfo = predicateBuilder.Build(expression);

            var invocationInfo = new InvocationInfo(
                typeof(IFoo).GetMethod("Execute", new Type[] { typeof(string) }),
                new[] { "SomeValue", "AnotherValue" });

            Assert.False(matchInfo.Matches(invocationInfo));
        }

        [Theory, MemberData(nameof(Equals_SameValue_ReturnsTrue_TestData))]
        public void Equals_SameValue_ReturnsTrue(LambdaExpression oneExpression, LambdaExpression anotherExpression)
        {
            var one = oneExpression.ToMatchInfo();
            var another = anotherExpression.ToMatchInfo();

            Assert.True(one.Equals(another));
        }

        public static IEnumerable<object[]> Equals_SameValue_ReturnsTrue_TestData()
        {
            yield return new object[] { GetFooAction(f => f.Execute("SomeValue")), GetFooAction(f => f.Execute("SomeValue")) };
            yield return new object[] { GetBazFunc(f => f.Property), GetBazFunc(f => f.Property) };
            yield return new object[] { GetBazFunc(f => f.Func()), GetBazFunc(f => f.Func()) };
            yield return new object[] { GetBazFunc(f => f.Func(1)), GetBazFunc(f => f.Func(1)) };
            yield return new object[] { GetBazAction(f => f.Method()), GetBazAction(f => f.Method()) };
        }

        [Theory, MemberData(nameof(Equals_DifferentValue_ReturnsFalse_TestData))]
        public void Equals_DifferentValue_ReturnsFalse(LambdaExpression oneExpression, LambdaExpression anotherExpression)
        {
            var one = oneExpression.ToMatchInfo();
            var another = anotherExpression.ToMatchInfo();

            Assert.False(one.Equals(another));
        }

        public static IEnumerable<object[]> Equals_DifferentValue_ReturnsFalse_TestData()
        {
            yield return new object[] { GetFooAction(f => f.Execute("SomeValue")), GetFooAction(f => f.Execute("AnotherValue")) };
            yield return new object[] { GetFooAction(f => f.Execute("AnotherValue")), GetFooAction(f => f.Execute("SomeValue")) };
            yield return new object[] { GetBazFunc(f => f.Property), GetBazFunc(f => f.Func()) };
            yield return new object[] { GetBazFunc(f => f.Func()), GetBazFunc(f => f.Property) };
            yield return new object[] { GetBazAction(f => f.Method()), GetFooAction(f => f.Execute("SomeValue")) };
        }

        static Expression<Func<IFoo, T>> GetFooFunc<T>(Expression<Func<IFoo, T>> expression) => expression;
        static Expression<Action<IFoo>> GetFooAction(Expression<Action<IFoo>> expression) => expression;
        static Expression<Action<IBaz>> GetBazAction(Expression<Action<IBaz>> expression) => expression;
        static Expression<Func<IBaz, T>> GetBazFunc<T>(Expression<Func<IBaz, T>> expression) => expression;
    }
}