namespace LightMock
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// A class that is capable of building a set of 
    /// lambda expressions used to match argument values.    
    /// </summary>
    public class PredicateBuilder : ExpressionVisitor, IPredicateBuilder
    {
        private Collection<LambdaExpression> lambdaExpressions = new Collection<LambdaExpression>();

        private MethodInfo method;

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
            method = ((MethodCallExpression)expression.Body).Method;
            Visit(expression);
            return new MatchInfo(method, lambdaExpressions.ToArray());
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            Type declaringType = node.Method.DeclaringType;
            if (declaringType.IsGenericType && declaringType.GetGenericTypeDefinition() == typeof(The<>))
            {
                LambdaExpression lambdaExpression = node.Find<LambdaExpression>(l => true).First();
                lambdaExpressions.Add(lambdaExpression);
            }
            else
            {
                //var constantExpression = Expression.Constant(node.Method, typeof(MethodInfo));
                //CreateEqualsExpression(constantExpression);
                if (node.Method == method)
                {
                    foreach (var argument in node.Arguments)
                    {
                        if (argument.NodeType == ExpressionType.Constant)
                        {
                            CreateEqualsExpression((ConstantExpression)argument);
                        }
                    }
                }
            }
            
            
            
            
            
            
            return base.VisitMethodCall(node);
        }

        private void CreateEqualsExpression(ConstantExpression constantExpression)
        {            
            ParameterExpression parameterExpression = Expression.Parameter(constantExpression.Type, "p");
            BinaryExpression equalExpression = Expression.Equal(parameterExpression, constantExpression);
            lambdaExpressions.Add(Expression.Lambda(equalExpression, parameterExpression));            
        }
    }
}