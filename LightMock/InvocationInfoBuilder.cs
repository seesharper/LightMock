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
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// An <see cref="ExpressionVisitor"/> that extracts 
    /// the target <see cref="MethodInfo"/> along with the 
    /// arguments used to invoke the method.
    /// </summary>
    internal class InvocationInfoBuilder : ExpressionVisitor, IInvocationInfoBuilder
    {
        private MethodInfo targetMethod;

        private Collection<object> arguments; 

        /// <summary>
        /// Returns an <see cref="InvocationInfo"/> instance that contains the target method 
        /// along with the arguments used to invoke the method.
        /// </summary>
        /// <param name="expression">The <see cref="Expression"/> from 
        /// which to extract the method and its arguments.</param>
        /// <returns>A <see cref="InvocationInfo"/> instance representing 
        /// the target method and the arguments used to invoke the method.</returns>
        public InvocationInfo Build(LambdaExpression expression)
        {
            var methodCallExpresssion = expression.Body as MethodCallExpression;
            if (methodCallExpresssion != null)
            {
                targetMethod = methodCallExpresssion.Method;
                arguments = new Collection<object>();
                Visit(expression);

                return new InvocationInfo(targetMethod, arguments.ToArray()); 
            }
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression != null)
            {
                var memberInfo = memberExpression.Member; 
                Visit(expression);
                return new InvocationInfo(memberInfo);
            }

            throw new NotSupportedException(string.Format("Expression type ({0}) not supported.", expression.Body.NodeType));
        }

        /// <summary>
        /// Visits the children of the <see cref="MethodCallExpression" />.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any sub expression was modified;
        /// otherwise, returns the original expression.</returns>
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            foreach (var argument in node.Arguments)
            {
                if (argument.NodeType == ExpressionType.Constant)
                {
                    arguments.Add(((ConstantExpression)argument).Value);
                }               
            }
                                                                     
            return base.VisitMethodCall(node);
        }  
    }
}