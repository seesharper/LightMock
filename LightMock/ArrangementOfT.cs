namespace LightMock
{
    using System.Linq.Expressions;

    public class Arrangement<TResult> : Arrangement
    {
        private TResult result;

        public Arrangement(LambdaExpression expression)
            : base(expression)
        {
        }

        public void Returns(TResult value)
        {
            result = value;            
        }

        internal override object Execute(object[] arguments)
        {
            base.Execute(arguments);            
            return result;
        }
    }
}