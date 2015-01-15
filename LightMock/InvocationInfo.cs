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

using System.Linq.Expressions;

namespace LightMock
{
    using System.Reflection;

    /// <summary>
    /// A class that represents a method invocations.
    /// </summary>
    public class InvocationInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvocationInfo"/> class.
        /// </summary>
        /// <param name="method">The invoked method.</param>
        /// <param name="arguments">The arguments used to invoked the method.</param>
        public InvocationInfo(MethodInfo method, object[] arguments)
        {
            Member = method;
            Arguments = arguments;
            ExpressionType = ExpressionType.Call;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvocationInfo"/> class.
        /// </summary>
        /// <param name="memberInfo">The invoked property member</param>
        public InvocationInfo(MemberInfo memberInfo)
        {
            Member = memberInfo;
            ExpressionType = ExpressionType.MemberAccess;
        }

        /// <summary>
        /// Get the invoked member.
        /// </summary>
        public MemberInfo Member { get; private set; }

        /// <summary>
        /// Gets the arguments used to invoke the method.
        /// </summary>
        public object[] Arguments { get; private set; }

        /// <summary>
        /// Gets the expression type.
        /// </summary>
        public ExpressionType ExpressionType { get; private set; }
    }
}