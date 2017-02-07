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
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using ExpressionReflect;

    /// <summary>
    /// A class that is used to match a method invocation.
    /// </summary>
    internal class MatchInfo
    {
        private readonly MemberInfo member;

        private readonly LambdaExpression[] matchExpressions;

        /// <summary>
        /// Initializes a new instance of the <see cref="MatchInfo"/> class.
        /// </summary>
        /// <param name="method">The target method to match.</param>
        /// <param name="matchExpressions">An <see cref="LambdaExpression"/> array that 
        /// represents matching argument values.</param>
        public MatchInfo(MethodInfo method, LambdaExpression[] matchExpressions)
        {
            this.member = method;
            this.matchExpressions = matchExpressions;
            ExpressionType = ExpressionType.Call;
        }  
        
        /// <summary>
        /// Initializes a new instance of the <see cref="MatchInfo"/> class.
        /// </summary>
        /// <param name="memberInfo">The target member to match.</param>
        public MatchInfo(MemberInfo memberInfo)
        {
            member = memberInfo;
            ExpressionType = ExpressionType.MemberAccess;
        }

        /// <summary>
        /// Determines if the <paramref name="invocationInfo"/> matches this <see cref="MatchInfo"/>.
        /// </summary>
        /// <param name="invocationInfo">The <see cref="InvocationInfo"/> to be matched.</param>
        /// <returns><b>True</b> if the <paramref name="invocationInfo"/> matches 
        /// this <see cref="MatchInfo"/>, otherwise, <b>False</b>.</returns>
        public bool Matches(InvocationInfo invocationInfo)
        {
            if (ExpressionType != invocationInfo.ExpressionType) return false;
            
            if (member != invocationInfo.Member)
            {
                return false;
            }

            if (ExpressionType == ExpressionType.MemberAccess) return true;

            if (matchExpressions.Length != invocationInfo.Arguments.Length)
            {
                return false;
            }

            return !matchExpressions.Where((t, i) => !(bool)t.Execute(invocationInfo.Arguments[i])).Any();
        }

        /// <summary>
        /// Gets the expression type.
        /// </summary>
        public ExpressionType ExpressionType { get; private set; }
    }
}