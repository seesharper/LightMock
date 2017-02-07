namespace LightMock.Tests
{
    using System;
    using System.Linq.Expressions;

   

    using LightMock;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Reflection;

    [TestClass]
    public class MatchInfoTests
    {
        [TestMethod]
        public void Matches_SameValue_ReturnsTrue()
        {
            var predicateBuilder = new MatchInfoBuilder();
            Expression<Action<IFoo>> expression = (f) => f.Execute("SomeValue");
            
            var matchInfo = predicateBuilder.Build(expression);            

            
            var invocationInfo = new InvocationInfo(
                typeof(IFoo).GetMethod("Execute", new Type[] { typeof(string) }),
                new[] { "SomeValue" });            

            Assert.IsTrue(matchInfo.Matches(invocationInfo));
        }

        [TestMethod]
        public void Matches_DifferentValue_ReturnsFalse()
        {
            var predicateBuilder = new MatchInfoBuilder();
            Expression<Action<IFoo>> expression = (f) => f.Execute("SomeValue");

            var matchInfo = predicateBuilder.Build(expression);

            var invocationInfo = new InvocationInfo(
                typeof(IFoo).GetMethod("Execute", new Type[] { typeof(string) }),
                new[] { "AnotherValue" });

            Assert.IsFalse(matchInfo.Matches(invocationInfo));
        }

        [TestMethod]
        public void Matches_SameValueDifferentMethod_ReturnsFalse()
        {
            var predicateBuilder = new MatchInfoBuilder();
            Expression<Action<IFoo>> expression = (f) => f.Execute("SomeValue");

            var matchInfo = predicateBuilder.Build(expression);

            var invocationInfo = new InvocationInfo(
                typeof(IBar).GetMethod("Execute", new Type[] { typeof(string) }),
                new[] { "SomeValue" });

            Assert.IsFalse(matchInfo.Matches(invocationInfo));
        }

        [TestMethod]
        public void Matches_ArgumentCountMismatch_ReturnsFalse()
        {
            var predicateBuilder = new MatchInfoBuilder();
            Expression<Action<IFoo>> expression = (f) => f.Execute("SomeValue");

            var matchInfo = predicateBuilder.Build(expression);

            var invocationInfo = new InvocationInfo(
                typeof(IFoo).GetMethod("Execute", new Type[] { typeof(string) }),
                new[] { "SomeValue", "AnotherValue" });

            Assert.IsFalse(matchInfo.Matches(invocationInfo));
        }
    }
}