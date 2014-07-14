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

        public string Execute()
        {
            return context.Invoke(f => f.Execute());
        }        
    }
}