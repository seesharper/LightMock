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
    /// A class that is capable of building a set of 
    /// lambda expressions used to match argument values.    
    /// </summary>
    internal class MatchInfoBuilder : ExpressionVisitor, IMatchInfoBuilder
    {
        private readonly Collection<LambdaExpression> lambdaExpressions = new Collection<LambdaExpression>();

        private MethodInfo targetMethod;

        /// <summary>
        /// Builds a new <see cref="MatchInfo"/> instance that is used to 
        /// match a method invocation.
        /// </summary>
        /// <param name="expression">The target <see cref="LambdaExpression"/>.</param>
        /// <returns>A <see cref="MatchInfo"/> instance that represents the target method
        /// and a <see cref="LambdaExpression"/> list where each element represents 
        /// matching an argument value.</returns>      
        public MatchInfo Build(LambdaExpression expression)
        {
            var methodCallExpresssion = expression.Body as MethodCallExpression;
            if (methodCallExpresssion != null)
            {
                targetMethod = methodCallExpresssion.Method;
                Visit(expression);
                return new MatchInfo(targetMethod, lambdaExpressions.ToArray());
            }
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression != null)
            {
                var memberInfo = memberExpression.Member;
                Visit(expression);
                return new MatchInfo(memberInfo);
            }

            throw new NotSupportedException(string.Format("Expression type ({0}) not supported.", expression.Body.NodeType));
        }
        
        /// <summary>
        /// Visits the <see cref="MethodCallExpression"/> and creates a match 
        /// expression for each argument.
        /// </summary>
        /// <param name="node">The <see cref="MethodCallExpression"/> to visit.</param>
        /// <returns><see cref="MethodCallExpression"/>.</returns>
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {                        
            if (NodeRepresentsTargetMethod(node))
            {
                CreateEqualsExpressions(node);
            }
                                                                                    
            return base.VisitMethodCall(node);
        }

        private void ExtractLambdaExpression(MethodCallExpression node)
        {
            var lambdaExpression = (LambdaExpression)node.Arguments[0];
            lambdaExpressions.Add(lambdaExpression);
        }
        
        private void CreateEqualsExpressions(MethodCallExpression node)
        {
            foreach (var argument in node.Arguments)
            {
                if (argument.NodeType == ExpressionType.Constant)
                {
                    CreateEqualsExpression((ConstantExpression)argument);
                }

                if (argument.NodeType == ExpressionType.Call)
                {
                    ExtractLambdaExpression((MethodCallExpression)argument);
                }
            }
        }

        private bool NodeRepresentsTargetMethod(MethodCallExpression node)
        {
            return node.Method == targetMethod;
        }

        private void CreateEqualsExpression(ConstantExpression constantExpression)
        {            
            ParameterExpression parameterExpression = Expression.Parameter(constantExpression.Type, "p");
            BinaryExpression equalExpression = Expression.Equal(parameterExpression, constantExpression);
            lambdaExpressions.Add(Expression.Lambda(equalExpression, parameterExpression));            
        }
    }
}