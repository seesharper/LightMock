namespace ExpressionReflect
{
	using System;

	internal static class TypeExtensions
	{
		internal static bool IsFunc(this Type type)
		{
			bool isFunc = false;

			if (type.IsGenericType)
			{
				Type definition = type.GetGenericTypeDefinition();
				isFunc = definition == typeof(Func<>) ||
					definition == typeof(Func<,>) ||
					definition == typeof(Func<,,>) ||
					definition == typeof(Func<,,,>) ||
					definition == typeof(Func<,,,,>);
			}

			return isFunc;
		}

		internal static bool IsAction(this Type type)
		{
			bool isAction = type == typeof(Action);

			if (type.IsGenericType)
			{
				Type definition = type.GetGenericTypeDefinition();

				isAction = isAction ||
					definition == typeof(Action<>) ||
					definition == typeof(Action<,>) ||
					definition == typeof(Action<,,>) ||
					definition == typeof(Action<,,,>);
			}

			return isAction;
		}

		internal static bool IsPredicate(this Type type)
		{
			bool isPredicate = false;

			if (type.IsGenericType)
			{
				Type definition = type.GetGenericTypeDefinition();
				isPredicate = definition == typeof(Predicate<>);
			}

			return isPredicate;
		}
	}
}