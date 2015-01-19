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
        /// Arranges a mocked method. 
        /// </summary>
        /// <param name="matchExpression">The match expression that describes where 
        /// this <see cref="Arrangement"/> will be applied.</param>
        /// <returns>A new <see cref="Arrangement"/> used to apply method behavior.</returns>
        Arrangement Arrange(Expression<Action<TMock>> matchExpression);

        /// <summary>
        /// Arranges a mocked method. 
        /// </summary>
        /// <typeparam name="TResult">The type of value returned from the mocked method.</typeparam>
        /// <param name="matchExpression">The match expression that describes where 
        /// this <see cref="Arrangement{TResult}"/> will be applied.</param>
        /// <returns>A new <see cref="Arrangement{TResult}"/> used to apply method behavior.</returns>
        Arrangement<TResult> Arrange<TResult>(Expression<Func<TMock, TResult>> matchExpression);

        /// <summary>
        /// Arranges a mocked property. 
        /// </summary>
        /// <typeparam name="TResult">The type of value returned from the mocked property.</typeparam>
        /// <param name="matchExpression">The match expression that describes where 
        /// this <see cref="PropertyArrangement{TResult}"/> will be applied.</param>
        /// <returns>A new <see cref="PropertyArrangement{TResult}"/> used to apply property behavior.</returns>
        PropertyArrangement<TResult> ArrangeProperty<TResult>(Expression<Func<TMock, TResult>> matchExpression);

        /// <summary>
        /// Verifies that the method represented by the <paramref name="matchExpression"/> has 
        /// been invoked.
        /// </summary>
        /// <param name="matchExpression">The <see cref="Expression{TDelegate}"/> that represents 
        /// the method invocation to be verified.</param>
        void Assert(Expression<Action<TMock>> matchExpression);

        /// <summary>
        /// Verifies that the method represented by the <paramref name="matchExpression"/> has 
        /// been invoked the specified number of <paramref name="invoked"/>.
        /// </summary>
        /// <param name="matchExpression">The <see cref="Expression{TDelegate}"/> that represents 
        /// the method invocation to be verified.</param>
        /// <param name="invoked">Specifies the number of times we expect the mocked method to be invoked.</param>
        void Assert(Expression<Action<TMock>> matchExpression, Invoked invoked);
    }
}