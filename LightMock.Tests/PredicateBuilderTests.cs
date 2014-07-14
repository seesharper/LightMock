namespace LightMock.Tests
{
    using System;
    using System.Linq.Expressions;

    using ExpressionReflect;

    using LightMock;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class PredicateBuilderTests
    {
        [TestMethod]
        public void Build_Constant_ReturnsTrueOnMatchingValue()
        {
            var predicateBuilder = new PredicateBuilder();
            Expression<Action<IFoo>> expression = (f) => f.Execute("SomeString");
            
            var matchInfo = predicateBuilder.Build(expression);
            
            Assert.IsTrue((bool)matchInfo.MatchExpressions[0].Execute("SomeString"));
        }

        [TestMethod]
        public void Build_Constant_ReturnsFalseOnNonMatchingValue()
        {
            var predicateBuilder = new PredicateBuilder();
            Expression<Action<IFoo>> expression = (f) => f.Execute("SomeString");

            var matchInfo = predicateBuilder.Build(expression);

            Assert.IsFalse((bool)matchInfo.MatchExpressions[0].Execute("AnotherValue"));
        }
    }
}