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
    using ExpressionReflect;
    

    /// <summary>
    /// A set of <see cref="LambdaExpression"/> extension methods.
    /// </summary>
    internal static class ExpressionExtensions
    {
        /// <summary>
        /// Simplifies the target <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="LambdaExpression"/> to simplify.</param>
        /// <returns>A simplified version of the target <paramref name="expression"/></returns>
        public static LambdaExpression Simplify(this LambdaExpression expression)
        {            
            expression = new MatchExpressionRewriter().Rewrite(expression);
            
            return
                (LambdaExpression)expression.PartialEval(
                    e => e.NodeType != ExpressionType.Parameter && e.NodeType != ExpressionType.Lambda);
        }

        /// <summary>
        /// Creates an <see cref="InvocationInfo"/> instance from the target <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="LambdaExpression"/> from which to create an <see cref="InvocationInfo"/> instance.</param>
        /// <returns><see cref="InvocationInfo"/>.</returns>
        public static InvocationInfo ToInvocationInfo(this LambdaExpression expression)
        {
            var invocationVisitor = new InvocationInfoBuilder();
            return invocationVisitor.Build(expression.Simplify());
        }

        /// <summary>
        /// Creates a <see cref="MatchInfo"/> instance from the target <paramref name="expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="LambdaExpression"/> from which to create a <see cref="MatchInfo"/> instance.</param>
        /// <returns><see cref="MatchInfo"/>.</returns>
        public static MatchInfo ToMatchInfo(this LambdaExpression expression)
        {                        
            var invocationVisitor = new MatchInfoBuilder();
            return invocationVisitor.Build(expression.Simplify());
        }                   
    }    
}