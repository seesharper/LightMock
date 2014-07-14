namespace LightMock
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// Represents a class that is capable of verifying method 
    /// invocations made to a mock object.
    /// </summary>
    /// <typeparam name="TMock">The target mock type.</typeparam>
    public interface IMockContext<TMock>
    {
        /// <summary>
        /// Verifies that the method represented by the <paramref name="matchExpression"/> has 
        /// been invoked.
        /// </summary>
        /// <param name="matchExpression">The <see cref="Expression{TDelegate}"/> that represents 
        /// the method invocation to be verified.</param>
        void Assert(Expression<Action<TMock>> matchExpression);
    }
}