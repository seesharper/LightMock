namespace LightMock.Tests
{
    public interface IFoo
    {
        void Execute(string value);
        void Execute(int first);
        void Execute(int first, int second);
        void Execute(int first, int second, int third);
        void Execute(int first, int second, int third, int fourth);
        string Execute();        
    }

    public interface IBar
    {
        void Execute(string value);
        string Execute();
    }
}