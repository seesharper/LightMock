using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Xunit;

namespace LightMock.Tests
{
    public class LambdaComparerTests
    {
        [Theory, MemberData(nameof(AreEqualTestData))]
        public void AreEqual(Expression left, Expression right)
        {
            var exc = new LambdaComparer();
            Assert.True(exc.Equals(left, right));
        }

        public static IEnumerable<object[]> AreEqualTestData()
        {
            yield return new object[] { null, null };
            yield return new object[] { GetBazAction(f => f.Method()), GetBazAction(f => f.Method()) };
            yield return new object[] { GetBazAction(f => f.Method(1)), GetBazAction(f => f.Method(1)) };
            yield return new object[] { GetBazFunc(f => f.Func()), GetBazFunc(f => f.Func()) };
            yield return new object[] { GetBazFunc(f => f.Func(1)), GetBazFunc(f => f.Func(1)) };
            yield return new object[] { GetBazFunc(f => f.Property), GetBazFunc(f => f.Property) };
        }

        [Theory, MemberData(nameof(AreNotEqualTestData))]
        public void AreNotEqual(Expression left, Expression right)
        {
            var exc = new LambdaComparer();
            Assert.False(exc.Equals(left, right));
        }

        public static IEnumerable<object[]> AreNotEqualTestData()
        {
            yield return new object[] { null, GetBazAction(f => f.Method(1)) };
            yield return new object[] { GetBazAction(f => f.Method(1)), null };
            yield return new object[] { GetBazAction(f => f.Method(1)), GetBazAction(f => f.Method()) };
            yield return new object[] { GetBazAction(f => f.Method()), GetBazAction(f => f.Method(1)) };
            yield return new object[] { GetBazAction(f => f.Method(1)), GetBazAction(f => f.Method(2)) };
            yield return new object[] { GetBazAction(f => f.Method(2)), GetBazAction(f => f.Method(1)) };
            yield return new object[] { GetBazFunc(f => f.Func()), GetBazFunc(f => f.Func(1)) };
            yield return new object[] { GetBazFunc(f => f.Func(1)), GetBazFunc(f => f.Func()) };
            yield return new object[] { GetBazFunc(f => f.Func(1)), GetBazFunc(f => f.Func(2)) };
            yield return new object[] { GetBazAction(f => f.Method()), GetBazFunc(f => f.Func()) };
            yield return new object[] { GetBazFunc(f => f.Func()), GetBazAction(f => f.Method()) };
            yield return new object[] { GetBazAction(f => f.Method()), GetBazFunc(f => f.Property) };
            yield return new object[] { GetBazFunc(f => f.Property), GetBazAction(f => f.Method()) };
            yield return new object[] { GetBazFunc(f => f.Func()), GetBazFunc(f => f.Property) };
            yield return new object[] { GetBazFunc(f => f.Property), GetBazFunc(f => f.Func()) };

            yield return new object[] { GetBazAction(f => f.Method()), GetQuuxAction(f => f.Method()) };
            yield return new object[] { GetBazAction(f => f.Method(1)), GetQuuxAction(f => f.Method(1)) };
            yield return new object[] { GetBazFunc(f => f.Func()), GetQuuxFunc(f => f.Func()) };
            yield return new object[] { GetBazFunc(f => f.Func(1)), GetQuuxFunc(f => f.Func(1)) };
            yield return new object[] { GetBazFunc(f => f.Property), GetQuuxFunc(f => f.Property) };
            yield return new object[] { GetQuuxAction(f => f.Method()), GetBazAction(f => f.Method()) };
            yield return new object[] { GetQuuxAction(f => f.Method(1)), GetBazAction(f => f.Method(1)) };
            yield return new object[] { GetQuuxFunc(f => f.Func()), GetBazFunc(f => f.Func()) };
            yield return new object[] { GetQuuxFunc(f => f.Func(1)), GetBazFunc(f => f.Func(1)) };
            yield return new object[] { GetQuuxFunc(f => f.Property), GetBazFunc(f => f.Property) };
        }

        static Expression<Action<IBaz>> GetBazAction(Expression<Action<IBaz>> expression)
            => expression;
        static Expression<Action<IQuux>> GetQuuxAction(Expression<Action<IQuux>> expression)
            => expression;

        static Expression<Func<IBaz, T>> GetBazFunc<T>(Expression<Func<IBaz, T>> expression)
            => expression;

        static Expression<Func<IQuux, T>> GetQuuxFunc<T>(Expression<Func<IQuux, T>> expression)
            => expression;
    }
}
