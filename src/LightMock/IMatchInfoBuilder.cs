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
    /// a set of lambda expressions used to match argument values.
    /// </summary>
    internal interface IMatchInfoBuilder
    {
        /// <summary>
        /// Builds a new <see cref="MatchInfo"/> instance that is used to 
        /// match a method invocation.
        /// </summary>
        /// <param name="expression">The target <see cref="LambdaExpression"/>.</param>
        /// <returns>A <see cref="MatchInfo"/> instance that represents the target method
        /// and a <see cref="LambdaExpression"/> list where each element represents 
        /// matching an argument value.</returns>      
        MatchInfo Build(LambdaExpression expression);
    }
}