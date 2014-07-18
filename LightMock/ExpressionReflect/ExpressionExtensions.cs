namespace ExpressionReflect
{
	using System.Linq.Expressions;

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