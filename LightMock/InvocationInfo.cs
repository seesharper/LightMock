namespace LightMock
{
    using System.Reflection;

    public class InvocationInfo
    {
        public InvocationInfo(MethodInfo method, object[] arguments)
        {
            Method = method;
            Arguments = arguments;
        }

        public MethodInfo Method { get; private set; }

        public object[] Arguments { get; private set; }        
    }
}