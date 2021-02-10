using System;

namespace LightMock.Tests
{
    interface IBaz
    {
        void Method();
        void Method(int a);
        int Func();
        int Func(int a);
        int Property { get; set; }
    }
}
