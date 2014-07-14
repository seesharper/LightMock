namespace LightMock
{
    using System;
    using System.Linq.Expressions;
    
    /// <summary>
    /// Represents a class that keeps track of method invocations made to 
    /// a manual mock object. 
    /// </summary>
    /// <typeparam name="TMock">The mock target type.</typeparam>
    public interface IInvocationContext<TMock>
    {
        /// <summary>
        /// Tracks that the method represented by the <paramref name="expression"/>
        /// has been invoked.
        /// </summary>
        /// <param name="expression">The <see cref="Expression{TDelegate}"/> that 
        /// represents the method that has been invoked.</param>
        void Invoke(Expression<Action<TMock>> expression);

        /// <summary>
        /// Tracks that the method represented by the <paramref name="expression"/>
        /// has been invoked.
        /// </summary>
        /// <typeparam name="TResult">The return type of the method that has been invoked.</typeparam>
        /// <param name="expression">The <see cref="Expression{TDelegate}"/> that 
        /// represents the method that has been invoked.</param>
        /// <returns>An instance of <typeparamref name="TResult"/> or possibly null 
        /// if <typeparamref name="TResult"/> a reference type.</returns>
        TResult Invoke<TResult>(Expression<Func<TMock, TResult>> expression);
    }
}