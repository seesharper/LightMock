namespace LightMock
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// An <see cref="ExpressionVisitor"/> that extracts 
    /// the target <see cref="MethodInfo"/> along with the 
    /// arguments used to invoke the method.
    /// </summary>
    internal class InvocationInfoBuilder : ExpressionVisitor, IInvocationInfoBuilder
    {
        private MethodInfo targetMethod;

        private Collection<object> arguments; 

        /// <summary>
        /// Returns an array that contains the target method 
        /// along with the arguments used to invoke the method.
        /// </summary>
        /// <param name="expression">The <see cref="Expression"/> from 
        /// which to extract the method and its arguments.</param>
        /// <returns></returns>
        public InvocationInfo GetInvocationInfo(LambdaExpression expression)
        {
            targetMethod = ((MethodCallExpression)expression.Body).Method;
            arguments = new Collection<object>();            
            Visit(expression);            
            
            return new InvocationInfo(targetMethod, arguments.ToArray());
        }

        /// <summary>
        /// Visits the children of the <see cref="MethodCallExpression" />.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any sub expression was modified;
        /// otherwise, returns the original expression.</returns>
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            foreach (var argument in node.Arguments)
            {
                if (argument.NodeType == ExpressionType.Constant)
                {
                    arguments.Add(((ConstantExpression)argument).Value);
                }               
            }   
                                                                     
            return base.VisitMethodCall(node);
        }  
    }
}