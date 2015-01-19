
using System.Linq.Expressions;

namespace LightMock
{
    /// <summary>
    /// A class that represents an arrangement of a mocked property that 
    /// returns a value of type <typeparamref name="TResult"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of the return value of the mocked property.</typeparam>
    public class PropertyArrangement<TResult> : Arrangement
    {
        private TResult result;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyArrangement{TResult}"/> class.
        /// </summary>
        /// <param name="expression">The <see cref="LambdaExpression"/> that specifies
        /// where to apply this <see cref="Arrangement"/>.</param>
        public PropertyArrangement(LambdaExpression expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Executes the arrangement.
        /// </summary>
        /// <param name="arguments">The arguments used to invoke the mocked method.</param>
        /// <returns>The registered return value, if any, otherwise, the default value.</returns>
        internal override object Execute(object[] arguments)
        {
            base.Execute(arguments);

            if (arguments != null && arguments.Length > 0) result = (TResult)arguments[0];

            return result;
        }
    }
}