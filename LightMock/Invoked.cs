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
        
        internal Invoked(Func<int, bool> evaluator)
        {
            this.evaluator = evaluator;
        }

        /// <summary>
        /// Specifies that the mocked method should be invoked exactly once.
        /// </summary>
        public static Invoked Once
        {
            get
            {
                return new Invoked(i => i == 1);
            }
        }

        public static Invoked AtLeast(int callCount)
        {
            return new Invoked(i => i > 0);
        }

        internal bool Verify(int callCount)
        {
            return evaluator(callCount);
        }
    }
}