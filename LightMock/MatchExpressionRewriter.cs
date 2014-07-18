namespace LightMock
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// An <see cref="ExpressionVisitor"/> that replaces references to the 
    /// <see cref="The{TValue}.IsAnyValue"/> with a <see cref="MethodCallExpression"/> 
    /// that represents calling the <see cref="The{TValue}.Is"/> method.    
    /// </summary>
    public class MatchExpressionRewriter : ExpressionVisitor
    {

        /// <summary>
        /// Replaces references to the <see cref="The{TValue}.IsAnyValue"/> with a <see cref="MethodCallExpression"/>
        /// that represents calling the <see cref="The{TValue}.Is"/> method.
        /// </summary>
        /// <param name="expression">The <see cref="LambdaExpression"/> to visit.</param>
        /// <returns><see cref="Expression"/>.</returns>
        public LambdaExpression Rewrite(LambdaExpression expression)
        {
            return (LambdaExpression)Visit(expression);
        }
                
        /// <summary>
        /// Replaces references to the <see cref="The{TValue}.IsAnyValue"/> with a <see cref="MethodCallExpression"/>
        /// that represents calling the <see cref="The{TValue}.Is"/> method.
        /// </summary>
        /// <param name="node">The <see cref="MemberExpression"/> to visit.</param>
        /// <returns><see cref="Expression"/>.</returns>
        protected override Expression VisitMember(MemberExpression node)
        {
            MemberInfo member = node.Member; 
            
            if (RepresentsIsAnyValueProperty(member))
            {                                
                return CreateMethodCallExpression(member);
            }
            
            return base.VisitMember(node);
        }

        private static bool RepresentsIsAnyValueProperty(MemberInfo member)
        {
            return member.Name == "IsAnyValue";
        }

        private static Expression CreateMethodCallExpression(MemberInfo member)
        {
            var parameterExpression = Expression.Parameter(GetParameterType(member), "v");
            var trueConstantExpression = Expression.Constant(true, typeof(bool));
            LambdaExpression trueExpression = Expression.Lambda(trueConstantExpression, parameterExpression);
            MethodCallExpression methodCallExpression = Expression.Call(GetIsMethod(member), trueExpression);
            return methodCallExpression;
        }

        private static MethodInfo GetIsMethod(MemberInfo member)
        {
            // ReSharper disable once PossibleNullReferenceException
	        return member.DeclaringType.GetMethod("Is");
        }

        private static Type GetParameterType(MemberInfo member)
        {
            // ReSharper disable once PossibleNullReferenceException
	        return member.DeclaringType.GetGenericArguments()[0];
        }
    }
}