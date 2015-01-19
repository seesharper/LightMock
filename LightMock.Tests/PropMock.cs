
namespace LightMock.Tests
{
    public interface IProp
    {
        string ReadOnlyProperty { get; }
        string TwoWayProperty { get; set; }
    }

    public class PropMock : IProp
    {
        private readonly IInvocationContext<IProp> _context;

        public PropMock(IInvocationContext<IProp> context)
        {
            _context = context;
        }

        public string ReadOnlyProperty
        {
            get
            {
                return _context.Invoke(x => x.ReadOnlyProperty);
            }
        }

        public string TwoWayProperty
        {
            get
            {
                return _context.Invoke(x => x.TwoWayProperty);
            }
            set
            {
                _context.InvokeSetter(x => x.TwoWayProperty, value);
            }
        }
    }
}