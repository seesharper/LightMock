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

using System;

namespace LightMock
{
    using System.Linq.Expressions;

    /// <summary>
    /// A class that represents an arrangement of a mocked method that 
    /// returns a value of type <typeparamref name="TResult"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of the return value of the mocked method.</typeparam>
    public class Arrangement<TResult> : Arrangement
    {
        private TResult result;

        private Func<object[], TResult> getResult;

        /// <summary>
        /// Initializes a new instance of the <see cref="Arrangement{TResult}"/> class.
        /// </summary>
        /// <param name="expression">The <see cref="LambdaExpression"/> that specifies
        /// where to apply this <see cref="Arrangement"/>.</param>
        public Arrangement(LambdaExpression expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Arranges for the mocked method to return a value of type <typeparamref name="TResult"/>.
        /// </summary>
        /// <param name="value">The value to be returned from the mocked method.</param>
        public void Returns(TResult value)
        {
            result = value;
        }

        /// <summary>
        /// Arranges for the mocked method to return a value of type <typeparamref name="TResult"/>. 
        /// Return value is set by the <paramref name="getResultFunc"/> when the mocked method is invoked.
        /// </summary>
        /// <param name="getResultFunc">The <see cref="Func{TResult}"/> is executed and returns value when the mocked method is invoked.</param>
        public void Returns(Func<TResult> getResultFunc)
        {
            getResult = args => (TResult)getResultFunc.DynamicInvoke(args);
        }

        /// <summary>
        /// Arranges for the mocked method to return a value of type <typeparamref name="TResult"/>. 
        /// Return value is set by the <paramref name="getResultFunc"/> when the mocked method is invoked.
        /// </summary>
        /// <typeparam name="T">The type of the first parameter.</typeparam>
        /// <param name="getResultFunc">The <see cref="Func{T, TResult}"/> is executed and returns value when the mocked method is invoked.</param>
        public void Returns<T>(Func<T, TResult> getResultFunc)
        {
            getResult = args => (TResult)getResultFunc.DynamicInvoke(args);
        }

        /// <summary>
        /// Arranges for the mocked method to return a value of type <typeparamref name="TResult"/>. 
        /// Return value is set by the <paramref name="getResultFunc"/> when the mocked method is invoked.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <param name="getResultFunc">The <see cref="Func{T1, T2, TResult}"/> is executed and returns value when the mocked method is invoked.</param>
        public void Returns<T1, T2>(Func<T1, T2, TResult> getResultFunc)
        {
            getResult = args => (TResult)getResultFunc.DynamicInvoke(args);
        }

        /// <summary>
        /// Arranges for the mocked method to return a value of type <typeparamref name="TResult"/>. 
        /// Return value is set by the <paramref name="getResultFunc"/> when the mocked method is invoked.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <typeparam name="T3">The type of the third parameter.</typeparam>
        /// <param name="getResultFunc">The <see cref="Func{T1, T2, T3, TResult}"/> is executed and returns value when the mocked method is invoked.</param>
        public void Returns<T1, T2, T3>(Func<T1, T2, T3, TResult> getResultFunc)
        {
            getResult = args => (TResult)getResultFunc.DynamicInvoke(args);
        }

        /// <summary>
        /// Arranges for the mocked method to return a value of type <typeparamref name="TResult"/>. 
        /// Return value is set by the <paramref name="getResultFunc"/> when the mocked method is invoked.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <typeparam name="T3">The type of the third parameter.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter.</typeparam>
        /// <param name="getResultFunc">The <see cref="Func{T1, T2, T3, T4, TResult}"/> is executed and returns value when the mocked method is invoked.</param>
        public void Returns<T1, T2, T3, T4>(Func<T1, T2, T3, T4, TResult> getResultFunc)
        {
            getResult = args => (TResult)getResultFunc.DynamicInvoke(args);
        }

        /// <summary>
        /// Executes the arrangement.
        /// </summary>
        /// <param name="arguments">The arguments used to invoke the mocked method.</param>
        /// <returns>The registered return value, if any, otherwise, the default value.</returns>
        internal override object Execute(object[] arguments)
        {
            base.Execute(arguments);

            if (getResult != null)
            {
                result = getResult(arguments);
            }

            return result;
        }
    }
}