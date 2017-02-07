/*****************************************************************************   
    The MIT License (MIT)

    Copyright (c) 2014 bernhard.richter@gmail.com

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE.
******************************************************************************    
    https://github.com/seesharper/LightMock
    http://twitter.com/bernhardrichter
******************************************************************************/
namespace LightMock
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// A class that represents the mock context for a given <typeparamref name="TMock"/> type.
    /// </summary>
    /// <typeparam name="TMock">The target mock type.</typeparam>
    public class MockContext<TMock> : IMockContext<TMock>, IInvocationContext<TMock>
    {
        private readonly List<InvocationInfo> invocations = new List<InvocationInfo>();        
        private readonly List<Arrangement> arrangements = new List<Arrangement>();

        /// <summary>
        /// Arranges a mocked method. 
        /// </summary>
        /// <param name="matchExpression">The match expression that describes where 
        /// this <see cref="Arrangement"/> will be applied.</param>
        /// <returns>A new <see cref="Arrangement"/> used to apply method behavior.</returns>
        public Arrangement Arrange(Expression<Action<TMock>> matchExpression)
        {
            var arrangement = new Arrangement(matchExpression);
            arrangements.Add(arrangement);
            return arrangement;
        }

        /// <summary>
        /// Arranges a mocked method. 
        /// </summary>
        /// <typeparam name="TResult">The type of value returned from the mocked method.</typeparam>
        /// <param name="matchExpression">The match expression that describes where 
        /// this <see cref="Arrangement{TResult}"/> will be applied.</param>
        /// <returns>A new <see cref="Arrangement{TResult}"/> used to apply method behavior.</returns>
        public Arrangement<TResult> Arrange<TResult>(Expression<Func<TMock, TResult>> matchExpression)
        {
            var arrangement = new Arrangement<TResult>(matchExpression);
            arrangements.Add(arrangement);
            return arrangement;
        }

        /// <summary>
        /// Arranges a mocked property. 
        /// </summary>
        /// <typeparam name="TResult">The type of value returned from the mocked property.</typeparam>
        /// <param name="matchExpression">The match expression that describes where 
        /// this <see cref="PropertyArrangement{TResult}"/> will be applied.</param>
        /// <returns>A new <see cref="PropertyArrangement{TResult}"/> used to apply property behavior.</returns>
        public PropertyArrangement<TResult> ArrangeProperty<TResult>(Expression<Func<TMock, TResult>> matchExpression)
        {
            var arrangement = new PropertyArrangement<TResult>(matchExpression);
            arrangements.Add(arrangement);
            return arrangement;
        }

        /// <summary>
        /// Verifies that the method represented by the <paramref name="matchExpression"/> has 
        /// been invoked.
        /// </summary>
        /// <param name="matchExpression">The <see cref="Expression{TDelegate}"/> that represents 
        /// the method invocation to be verified.</param>
        public void Assert(Expression<Action<TMock>> matchExpression)
        {
            Assert(matchExpression, Invoked.AtLeast(1));            
        }

        /// <summary>
        /// Verifies that the method represented by the <paramref name="matchExpression"/> has 
        /// been invoked the specified number of <paramref name="invoked"/>.
        /// </summary>
        /// <param name="matchExpression">The <see cref="Expression{TDelegate}"/> that represents 
        /// the method invocation to be verified.</param>
        /// <param name="invoked">Specifies the number of times we expect the mocked method to be invoked.</param>
        public void Assert(Expression<Action<TMock>> matchExpression, Invoked invoked)
        {
            var matchInfo = matchExpression.ToMatchInfo();
            var callCount = invocations.Count(matchInfo.Matches);
                        
            if (!invoked.Verify(callCount))
            {
                throw new InvalidOperationException(string.Format("The method {0} was called {1} times", matchExpression.Simplify(), callCount));
            }
        }

        /// <summary>
        /// Tracks that the method represented by the <paramref name="expression"/>
        /// has been invoked.
        /// </summary>
        /// <param name="expression">The <see cref="Expression{TDelegate}"/> that 
        /// represents the method that has been invoked.</param>
        void IInvocationContext<TMock>.Invoke(Expression<Action<TMock>> expression)
        {
            var invocationInfo = expression.ToInvocationInfo();
            invocations.Add(invocationInfo);
           
            var arrangement = arrangements.FirstOrDefault(a => a.Matches(invocationInfo));
            if (arrangement != null)
            {
                arrangement.Execute(invocationInfo.Arguments);
            }            
        }

        /// <summary>
        /// Tracks that the method represented by the <paramref name="expression"/>
        /// has been invoked.
        /// </summary>
        /// <typeparam name="TResult">The return type of the method that has been invoked.</typeparam>
        /// <param name="expression">The <see cref="Expression{TDelegate}"/> that 
        /// represents the method that has been invoked.</param>
        /// <returns>An instance of <typeparamref name="TResult"/> or possibly null 
        /// if <typeparamref name="TResult"/> a reference type.</returns>
        TResult IInvocationContext<TMock>.Invoke<TResult>(Expression<Func<TMock, TResult>> expression)
        {
            var invocationInfo = expression.ToInvocationInfo();
            invocations.Add(invocationInfo);

            var arrangement = arrangements.FirstOrDefault(a => a.Matches(invocationInfo));
            if (arrangement != null)
            {
                return (TResult)arrangement.Execute(invocationInfo.Arguments);
            }

            return default(TResult);
        }

        /// <summary>
        /// Tracks that the setter represented by the <paramref name="expression"/>
        /// has been invoked.
        /// </summary>
        /// <param name="expression">The <see cref="Expression{TDelegate}"/> that 
        /// represents the setter that has been invoked.</param>
        /// <param name="value">The value</param>
        void IInvocationContext<TMock>.InvokeSetter<TResult>(Expression<Func<TMock, TResult>> expression, object value)
        {
            var invocationInfo = expression.ToInvocationInfo();
            invocations.Add(invocationInfo);

            var arrangement = arrangements.FirstOrDefault(a => a.Matches(invocationInfo));
            if (arrangement != null)
            {
                arrangement.Execute(new[] { value });
            }
        }

    }
}