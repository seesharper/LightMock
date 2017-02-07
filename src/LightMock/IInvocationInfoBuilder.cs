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
    using System.Linq.Expressions;

    /// <summary>
    /// Represents a class that is capable of building 
    /// an <see cref="InvocationInfo"/> that represents information 
    /// about a method invocation.
    /// </summary>
    internal interface IInvocationInfoBuilder
    {
        /// <summary>
        /// Returns an <see cref="InvocationInfo"/> instance that contains the target method 
        /// along with the arguments used to invoke the method.
        /// </summary>
        /// <param name="expression">The <see cref="Expression"/> from 
        /// which to extract the method and its arguments.</param>
        /// <returns>A <see cref="InvocationInfo"/> instance representing 
        /// the target method and the arguments used to invoke the method.</returns>
        InvocationInfo Build(LambdaExpression expression);
    }
}