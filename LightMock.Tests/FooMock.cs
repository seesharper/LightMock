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

        public void Execute(string value)
        {
            context.Invoke(f => f.Execute(value));
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
    }
}