namespace LightMock
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// A class that is capable of building a set of 
    /// lambda expressions used to match argument values.    
    /// </summary>
    internal class MatchInfoBuilder : ExpressionVisitor, IMatchInfoBuilder
    {
        private readonly Collection<LambdaExpression> lambdaExpressions = new Collection<LambdaExpression>();

        private MethodInfo targetMethod;

        /// <summary>
        /// Builds a <see cref="LambdaExpression"/> for each 
        /// argument in the method call where each <see cref="LambdaExpression"/>
        /// represents matching an argument value.
        /// </summary>
        /// <param name="expression">The target <see cref="LambdaExpression"/>.</param>
        /// <returns>An array where each element is a <see cref="LambdaExpression"/> 
        /// representing matching an argument value.</returns>
        public MatchInfo Build(LambdaExpression expression)
        {
            targetMethod = ((MethodCallExpression)expression.Body).Method;
            Visit(expression);
            return new MatchInfo(targetMethod, lambdaExpressions.ToArray());
        }
        
        /// <summary>
        /// Visits the <see cref="MethodCallExpression"/> and creates a match 
        /// expression for each argument.
        /// </summary>
        /// <param name="node">The <see cref="MethodCallExpression"/> to visit.</param>
        /// <returns><see cref="MethodCallExpression"/>.</returns>
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {                        
            if (NodeRepresentsTargetMethod(node))
            {
                CreateEqualsExpressions(node);
            }
                                                                                    
            return base.VisitMethodCall(node);
        }

        private void ExtractLambdaExpression(MethodCallExpression node)
        {
            LambdaExpression lambdaExpression = node.Find<LambdaExpression>(l => true).First();
            lambdaExpressions.Add(lambdaExpression);
        }
        
        private void CreateEqualsExpressions(MethodCallExpression node)
        {
            foreach (var argument in node.Arguments)
            {
                if (argument.NodeType == ExpressionType.Constant)
                {
                    CreateEqualsExpression((ConstantExpression)argument);
                }

                if (argument.NodeType == ExpressionType.Call)
                {
                    ExtractLambdaExpression(node);
                }
            }
        }

        private bool NodeRepresentsTargetMethod(MethodCallExpression node)
        {
            return node.Method == targetMethod;
        }

        private void CreateEqualsExpression(ConstantExpression constantExpression)
        {            
            ParameterExpression parameterExpression = Expression.Parameter(constantExpression.Type, "p");
            BinaryExpression equalExpression = Expression.Equal(parameterExpression, constantExpression);
            lambdaExpressions.Add(Expression.Lambda(equalExpression, parameterExpression));            
        }
    }
}