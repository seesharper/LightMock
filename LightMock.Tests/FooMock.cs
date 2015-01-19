namespace LightMock.Tests
{
    using LightMock;

    public class FooMock : IFoo
    {
        private readonly IInvocationContext<IFoo> context;

        public FooMock(IInvocationContext<IFoo> context)
        {
            this.context = context;
        }

        public string Execute(string value)
        {
            return context.Invoke(f => f.Execute(value));
        }

        public void Execute(int first)
        {
            context.Invoke(f => f.Execute(first));
        }

        public void Execute(int first, int second)
        {
            context.Invoke(f => f.Execute(first, second));
        }

        public void Execute(int first, int second, int third)
        {
            context.Invoke(f => f.Execute(first, second, third));
        }

        public void Execute(int first, int second, int third, int fourth)
        {
            context.Invoke(f => f.Execute(first, second, third, fourth));
        }

        public string Execute()
        {
            return context.Invoke(f => f.Execute());
        }

        public string Execute(string first, string second)
        {
            return context.Invoke(f => f.Execute(first, second));
        }

        public string Execute(string first, string second, string third)
        {
            return context.Invoke(f => f.Execute(first, second, third));
        }

        public string Execute(string first, string second, string third, string fourth)
        {
            return context.Invoke(f => f.Execute(first, second, third, fourth));
        }

        public byte[] Execute(byte[] array)
        {
            return context.Invoke((f => f.Execute(array)));
        }
    }
}