namespace LightMock.Tests
{
    public interface IFoo
    {
        void Execute(int first);
        void Execute(int first, int second);
        void Execute(int first, int second, int third);
        void Execute(int first, int second, int third, int fourth);
        void Execute(int first, int second, int third, int fourth, int fifth);
        void Execute(int first, int second, int third, int fourth, int fifth, int sixth);

        string Execute();
        string Execute(string value);
        string Execute(string first, string second);
        string Execute(string first, string second, string third);
        string Execute(string first, string second, string third, string fourth);

        byte[] Execute(byte[] array);
    }

    public interface IBar
    {
        void Execute(string value);
        string Execute();
    }
}