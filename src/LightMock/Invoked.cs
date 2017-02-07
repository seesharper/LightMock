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
    /// A class used to specify the number of times 
    /// we expect a method to be invoked.
    /// </summary>
    public class Invoked
    {
        private readonly Func<int, bool> evaluator;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Invoked"/> class.
        /// </summary>
        /// <param name="evaluator">A function delegate used to evaluate the actual number 
        /// of method invocations.</param>
        internal Invoked(Func<int, bool> evaluator)
        {
            this.evaluator = evaluator;
        }

        /// <summary>
        /// Gets a new <see cref="Invoked"/> specifying that method should be invoked exactly once.
        /// </summary>
        public static Invoked Once
        {
            get
            {
                return new Invoked(i => i == 1);
            }
        }

        /// <summary>
        /// Gets a new <see cref="Invoked"/> specifying that method should never have been invoked.
        /// </summary>
        public static Invoked Never
        {
            get
            {
                return new Invoked(i => i == 0);
            }
        }

        /// <summary>
        /// Specifies that the mocked method should be invoked at least a given number of times.
        /// </summary>
        /// <param name="callCount">The expected number of times for the mocked method to be invoked.</param>
        /// <returns>An <see cref="Invoked"/> instance that represent the expected number of invocations.</returns>
        public static Invoked AtLeast(int callCount)
        {
            return new Invoked(i => i >= callCount);
        }

        /// <summary>
        /// Specifies that the mocked method should be invoked a given number of times.
        /// </summary>
        /// <param name="callCount">The expected number of times for the mocked method to be invoked.</param>
        /// <returns>An <see cref="Invoked"/> instance that represent the expected number of invocations.</returns>
        public static Invoked Exactly(int callCount)
        {
            return new Invoked(i => i == callCount);
        }

        /// <summary>
        /// Verifies that the actual <paramref name="callCount"/> matches the expected call count.
        /// </summary>
        /// <param name="callCount">The actual number of times the mocked method has been invoked.</param>
        /// <returns><b>True</b> if the actual <paramref name="callCount"/> matches the expected 
        /// call count, otherwise, <b>False</b>.</returns>
        internal bool Verify(int callCount)
        {
            return evaluator(callCount);
        }
    }
}