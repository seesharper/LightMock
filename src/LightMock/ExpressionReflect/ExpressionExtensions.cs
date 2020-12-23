namespace ExpressionReflect
{
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;

    [ExcludeFromCodeCoverage]
    public static class ExpressionExtensions
    {
        public static object Execute(this Expression expression, params object[] values)
        {
            ExpressionReflectionExecutor visitor = new ExpressionReflectionExecutor(expression);
            object result = visitor.Execute(values);
            return result;
        }
    }
}