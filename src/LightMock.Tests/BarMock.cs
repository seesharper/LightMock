namespace LightMock.Tests
{
    public class BarMock : IBar
    {
        private readonly IInvocationContext<IFoo> context;

        public BarMock(IInvocationContext<IFoo> context)
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