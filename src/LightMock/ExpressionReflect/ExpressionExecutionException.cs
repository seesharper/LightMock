namespace ExpressionReflect
{
	using System;

	public class ExpressionExecutionException : Exception
	{
		public ExpressionExecutionException()
		{
		}

		public ExpressionExecutionException(string message)
			: base(message)
		{
		}

		public ExpressionExecutionException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}