namespace LightMock
{
    using System;

    public class The<TValue>
    {        
        public static TValue Is(Func<TValue, bool> match)
        {
            return default(TValue);
        }

        public static TValue IsAnyValue
        {
            get
            {
                return default(TValue);
            }                        
        }
    }
}