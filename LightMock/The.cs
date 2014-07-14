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

    /// <summary>
    /// Specifies an argument match.
    /// </summary>
    /// <typeparam name="TValue">The type of the target parameter.</typeparam>
    public class The<TValue>
    {
        /// <summary>
        /// Specifies that the argument value can be any value of <typeparamref name="TValue"/>.
        /// </summary>
        public static TValue IsAnyValue
        {
            get
            {
                return default(TValue);
            }
        }
        
        /// <summary>
        /// Specifies that the argument value must match the given <paramref name="predicate"/>.
        /// </summary>
        /// <param name="predicate">A <see cref="Func{Tvalue, TResult}"/> that represents the match predicate.</param>
        /// <returns>default(<typeparamref name="TValue"/>)</returns>
        public static TValue Is(Func<TValue, bool> predicate)
        {
            return default(TValue);
        }
        
    }
}