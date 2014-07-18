namespace ExpressionReflect
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reflection;

	/// <summary>
	/// An expression visitor that translates the expression tree to reflection calls.
	/// </summary>
	internal sealed class ExpressionReflectionExecutor : ExpressionVisitor
	{
		private IDictionary<string, object> args = new Dictionary<string, object>();
		private readonly Stack<object> data = new Stack<object>();
		private readonly Stack<LambdaExpression> nestedLambdas = new Stack<LambdaExpression>();

		private readonly Expression targetExpression;
		private object[] passedArgumentValues;


		public ExpressionReflectionExecutor(Expression targetExpression)
		{
			this.targetExpression = targetExpression;
		}

		/// <summary>
		/// The entry point for the evaluation.
		/// </summary>
		/// <param name="returnsValue">Flag, indicating if the expression returns a value. The default is <c>true</c>.</param>
		/// <returns>The result of the expression.</returns>
		internal object Execute(object[] passedArgumentValues, bool returnsValue = true)
		{
			this.passedArgumentValues = passedArgumentValues;
			Initialize();
			this.Visit(((LambdaExpression)targetExpression).Body);

			if (this.data.Count > 1)
			{
				throw new ExpressionExecutionException("The stack contained too much elements.");
			}
			if (returnsValue && this.data.Count < 1)
			{
				throw new ExpressionExecutionException("The stack contained too few elements.");
			}

			object value = null;

			if (returnsValue)
			{
				value = this.GetValueFromStack();
			}

			return value;
		}

		protected override Expression VisitLambda<T>(Expression<T> node)
		{

			Delegate @delegate = null;

			string methodName = null;

			Type type = node.Type;
			if (type.IsFunc())
			{
				methodName = "Func";
			}
			else if (type.IsAction())
			{
				methodName = "Action";
			}
			else if (type.IsPredicate())
			{
				methodName = "Predicate";
			}

			if (string.IsNullOrWhiteSpace(methodName))
			{
				throw new ExpressionExecutionException(string.Format("No wrapper method available for delegate type '{0}'", type.Name));
			}

			var executor = new ExpressionReflectionExecutor(node);

			Type[] genericArguments = type.GetGenericArguments();
			MethodInfo methodInfo = this.FindMethod(methodName, genericArguments);
			@delegate = Delegate.CreateDelegate(type, executor, methodInfo);

			return this.VisitConstant(Expression.Constant(@delegate));


		}

		protected override Expression VisitParameter(ParameterExpression p)
		{
			Expression expression = base.VisitParameter(p);

			object argument = this.args[p.Name];
			this.data.Push(argument);

			return expression;
		}

		protected override Expression VisitMember(MemberExpression node)
		{
			Expression expression = base.VisitMember(node);
			MemberInfo memberInfo = node.Member;

			if (memberInfo is PropertyInfo)
			{
				object target = null;
				if (!((PropertyInfo)memberInfo).GetGetMethod().IsStatic)
				{
					target = this.GetValueFromStack();
				}
				PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
				object value = propertyInfo.GetValue(target, null);
				this.data.Push(value);
			}
			if (memberInfo is FieldInfo)
			{
				object target = null;
				if (!((FieldInfo)memberInfo).IsStatic)
				{
					target = this.GetValueFromStack();
				}
				FieldInfo fieldInfo = (FieldInfo)memberInfo;
				object value = fieldInfo.GetValue(target);
				this.data.Push(value);
			}

			return expression;
		}

		protected override Expression VisitMethodCall(MethodCallExpression m)
		{
			Expression expression = base.VisitMethodCall(m);

			object target = null;
			object[] parameterValues = this.GetValuesFromStack(m.Arguments.Count);

			if (m.Object != null)
			{
				target = this.GetValueFromStack();
			}

			// If expression is null the call is static, so the target must and will be null.
			MethodInfo methodInfo = m.Method;
			object value = methodInfo.Invoke(target, parameterValues);

			this.data.Push(value);

			// Remove the nested lambda expression from stack after method execution.
			if (this.nestedLambdas.Any())
			{
				this.nestedLambdas.Pop();
			}

			return expression;
		}

		protected override Expression VisitInvocation(InvocationExpression node)
		{
			Expression expression = base.VisitInvocation(node);

			object value = null;

			if (node.Expression is MemberExpression)
			{
				object[] arguments = this.GetValuesFromStack(node.Arguments.Count);

				// Use the delegate on the stack. The constant expression visitor pushed it there.
				value = this.GetValueFromStack();

				if (value is Delegate)
				{
					Delegate del = (Delegate)value;
					value = del.DynamicInvoke(arguments);
				}
			}

			this.data.Push(value);

			return expression;
		}

		protected override Expression VisitNew(NewExpression nex)
		{
			Expression expression = base.VisitNew(nex);

			ConstructorInfo constructorInfo = nex.Constructor;
			object[] parameterValues = this.GetValuesFromStack(nex.Arguments.Count);

			object value = constructorInfo.Invoke(parameterValues.ToArray());
			this.data.Push(value);

			return expression;
		}

		protected override Expression VisitBinary(BinaryExpression b)
		{
			Expression expression = base.VisitBinary(b);

			object value;

			object[] values = this.GetValuesFromStack(2);

			MethodInfo methodInfo = b.Method;
			if (methodInfo != null)
			{
				// If an operator method is available use it.
				value = methodInfo.Invoke(null, values);
			}
			else
			{
				switch (b.NodeType)
				{
					case ExpressionType.Add:
						value = Convert.ToDouble(values.First()) + Convert.ToDouble(values.Last());
						break;
					case ExpressionType.AddChecked:
						value = checked(Convert.ToDouble(values.First()) + Convert.ToDouble(values.Last()));
						break;
					case ExpressionType.Subtract:
						value = Convert.ToDouble(values.First()) - Convert.ToDouble(values.Last());
						break;
					case ExpressionType.SubtractChecked:
						value = checked(Convert.ToDouble(values.First()) - Convert.ToDouble(values.Last()));
						break;
					case ExpressionType.Multiply:
						value = Convert.ToDouble(values.First()) * Convert.ToDouble(values.Last());
						break;
					case ExpressionType.MultiplyChecked:
						value = checked(Convert.ToDouble(values.First()) * Convert.ToDouble(values.Last()));
						break;
					case ExpressionType.Divide:
						value = Convert.ToDouble(values.First()) / Convert.ToDouble(values.Last());
						break;
					case ExpressionType.Modulo:
						value = Convert.ToDouble(values.First()) % Convert.ToDouble(values.Last());
						break;
					case ExpressionType.Equal:
						value = Equals(values.First(), values.Last());
						break;
					case ExpressionType.NotEqual:
						value = !(values.First().Equals(values.Last()));
						break;
					case ExpressionType.And:
						value = Convert.ToBoolean(values.First()) & Convert.ToBoolean(values.Last());
						break;
					case ExpressionType.AndAlso:
						value = Convert.ToBoolean(values.First()) && Convert.ToBoolean(values.Last());
						break;
					case ExpressionType.Or:
						value = Convert.ToBoolean(values.First()) | Convert.ToBoolean(values.Last());
						break;
					case ExpressionType.OrElse:
						value = Convert.ToBoolean(values.First()) || Convert.ToBoolean(values.Last());
						break;
					case ExpressionType.ExclusiveOr:
						value = Convert.ToBoolean(values.First()) ^ Convert.ToBoolean(values.Last());
						break;
					case ExpressionType.LessThan:
						value = Convert.ToDouble(values.First()) < Convert.ToDouble(values.Last());
						break;
					case ExpressionType.LessThanOrEqual:
						value = Convert.ToDouble(values.First()) <= Convert.ToDouble(values.Last());
						break;
					case ExpressionType.GreaterThan:
						value = Convert.ToDouble(values.First()) > Convert.ToDouble(values.Last());
						break;
					case ExpressionType.GreaterThanOrEqual:
						value = Convert.ToDouble(values.First()) >= Convert.ToDouble(values.Last());
						break;
					case ExpressionType.RightShift:
						value = Convert.ToInt64(values.First()) >> Convert.ToInt32(values.Last());
						break;
					case ExpressionType.LeftShift:
						value = Convert.ToInt64(values.First()) << Convert.ToInt32(values.Last());
						break;
					case ExpressionType.Coalesce:
						value = values.First() ?? values.Last();
						break;
					case ExpressionType.ArrayIndex:
						object[] array = (object[])values.First();
						value = array[Convert.ToInt64(values.Last())];
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}

			Type type = b.Type;
			value = Convert.ChangeType(value, type, CultureInfo.InvariantCulture);
			this.data.Push(value);

			return expression;
		}

		protected override Expression VisitTypeBinary(TypeBinaryExpression b)
		{
			Expression expression = base.VisitTypeBinary(b);

			object target = this.GetValueFromStack();
			Type isType = b.TypeOperand;

			bool value = isType.IsInstanceOfType(target);
			this.data.Push(value);

			return expression;
		}

		protected override Expression VisitUnary(UnaryExpression u)
		{
			Expression expression = base.VisitUnary(u);

			object result;
			object value = this.GetValueFromStack();

			MethodInfo methodInfo = u.Method;
			if (methodInfo != null)
			{
				// If an operator method is available use it.
				result = methodInfo.Invoke(null, new object[] { value });
			}
			else
			{
				switch (u.NodeType)
				{
					case ExpressionType.Negate:
						result = -Convert.ToDouble(value);
						break;
					case ExpressionType.NegateChecked:
						result = checked(-Convert.ToDouble(value));
						break;
					case ExpressionType.Not:
						if (value is bool)
						{
							result = !Convert.ToBoolean(value);
						}
						else
						{
							result = ~Convert.ToInt64(value);
						}
						break;
					//case ExpressionType.Quote:
					case ExpressionType.Convert:
						result = Convert.ChangeType(value, u.Type, CultureInfo.InvariantCulture);
						break;
					case ExpressionType.ConvertChecked:
						result = checked(Convert.ChangeType(value, u.Type, CultureInfo.InvariantCulture));
						break;
					case ExpressionType.ArrayLength:
						result = ((object[])value).Length;
						break;
					case ExpressionType.TypeAs:
						try
						{
							result = Convert.ChangeType(value, u.Type, CultureInfo.InvariantCulture);
						}
						catch (InvalidCastException)
						{
							result = null;
						}
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}

			result = Convert.ChangeType(result, u.Type, CultureInfo.InvariantCulture);
			this.data.Push(result);

			return expression;
		}

		protected override Expression VisitConditional(ConditionalExpression c)
		{
			Expression expression = base.VisitConditional(c);

			object ifFalse = this.GetValueFromStack();
			object ifTrue = this.GetValueFromStack();
			bool test = (bool)this.GetValueFromStack();

			object value = test ? ifTrue : ifFalse;
			this.data.Push(value);

			return expression;
		}

		protected override Expression VisitConstant(ConstantExpression c)
		{
			this.data.Push(c.Value);
			return base.VisitConstant(c);
		}

		protected override Expression VisitNewArray(NewArrayExpression na)
		{
			Expression expression = base.VisitNewArray(na);

			Array newArray;
			Type type = na.Type.GetElementType();

			switch (na.NodeType)
			{
				case ExpressionType.NewArrayBounds:
					int length = (int)this.GetValueFromStack();
					newArray = Array.CreateInstance(type, length);
					break;
				case ExpressionType.NewArrayInit:
					Array arrayValues = this.GetValuesFromStack(na.Expressions.Count);
					newArray = Array.CreateInstance(type, arrayValues.Length);
					Array.Copy(arrayValues, newArray, arrayValues.Length);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			this.data.Push(newArray);

			return expression;
		}

		protected override Expression VisitMemberInit(MemberInitExpression init)
		{
			Expression expression = base.VisitMemberInit(init);

			// Step 1: Get all values for the initialization
			object[] values = this.GetValuesFromStack(init.Bindings.Count);

			// Set 2: Get target from stack
			object target = this.GetValueFromStack();

			// Set 3: Initialize the properties.
			for (int index = 0; index < init.Bindings.Count; index++)
			{
				MemberBinding binding = init.Bindings[index];
				MemberInfo memberInfo = binding.Member;
				if (memberInfo is PropertyInfo)
				{
					PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
					object value = values[index];
					propertyInfo.SetValue(target, value, null);
				}
				else if (memberInfo is FieldInfo)
				{
					FieldInfo fieldInfo = (FieldInfo)memberInfo;
					object value = values[index];
					fieldInfo.SetValue(target, value);
				}
			}

			// Set 4 : Put initialized instance back on the stack.
			this.data.Push(target);

			return expression;
		}

		protected override Expression VisitListInit(ListInitExpression init)
		{
			Expression expression = base.VisitListInit(init);

			// Set 1: Get all values for initialization
			int initializerArgumentCount = init.Initializers.First().Arguments.Count;
			int initializerCount = init.Initializers.Count;
			object[] values = this.GetValuesFromStack(initializerCount * initializerArgumentCount);

			// Set 2: Get target from stack
			object target = this.GetValueFromStack();

			// Set 3: Add the values
			for (int i = 0; i < initializerCount; i++)
			{
				ElementInit initializer = init.Initializers[i];

				object[] argumentValues = new object[initializerArgumentCount];
				for (int j = 0; j < initializerArgumentCount; j++)
				{
					int index = (i * initializerArgumentCount) + j;
					object arg = values[index];
					argumentValues[j] = arg;
				}

				MethodInfo methodInfo = initializer.AddMethod;
				methodInfo.Invoke(target, argumentValues);
			}

			// Set 4: Put target back on the stack
			this.data.Push(target);

			return expression;
		}

		private object[] GetValuesFromStack(int count)
		{
			IList<object> parameterValues = new List<object>();

			for (int i = 0; i < count; i++)
			{
				object parameterValue = this.GetValueFromStack();
				parameterValues.Add(parameterValue);
			}

			return parameterValues.Reverse().ToArray();
		}

		/// <summary>
		/// Gets a single value from the stack.
		/// </summary>
		/// <returns>The element.</returns>
		private object GetValueFromStack()
		{
			object parameterValue = this.data.Pop();
			return parameterValue;
		}

		private void Initialize()
		{
			this.args = InitializeArgs((LambdaExpression)this.targetExpression, this.passedArgumentValues);
		}

		private static IDictionary<string, object> InitializeArgs(LambdaExpression lambdaExpression, object[] parameterValues)
		{
			IDictionary<string, object> arguments = new Dictionary<string, object>();

			int index = 0;
			foreach (ParameterExpression parameter in lambdaExpression.Parameters)
			{
				string name = parameter.Name;
				arguments[name] = parameterValues[index++];
			}

			return arguments;
		}

		private MethodInfo FindMethod(string name, Type[] genericArguments)
		{
			MethodInfo result = null;

			IEnumerable<MethodInfo> methods = this.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).Where(x => x.Name == name);
			foreach (MethodInfo method in methods)
			{
				// create the generic method
				if (method.GetGenericArguments().Count() == genericArguments.Count())
				{
					result = method.IsGenericMethod
						? method.MakeGenericMethod(genericArguments)
						: method;
					break;
				}
			}

			return result;
		}

		private TResult Func<T, TResult>(T arg)
		{
			return (TResult)this.ExecuteReflector(arg);
		}

		private TResult Func<T1, T2, TResult>(T1 arg1, T2 arg2)
		{
			return (TResult)this.ExecuteReflector(arg1, arg2);
		}

		private TResult Func<T1, T2, T3, TResult>(T1 arg1, T2 arg2, T3 arg3)
		{
			return (TResult)this.ExecuteReflector(arg1, arg2, arg3);
		}

		private TResult Func<T1, T2, T3, T4, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
		{
			return (TResult)this.ExecuteReflector(arg1, arg2, arg3, arg4);
		}

		private void Action()
		{
			this.ExecuteReflector();
		}

		private void Action<T>(T arg)
		{
			this.ExecuteReflector(arg);
		}

		private void Action<T1, T2>(T1 arg1, T2 arg2)
		{
			this.ExecuteReflector(arg1, arg2);
		}

		private void Action<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3)
		{
			this.ExecuteReflector(arg1, arg2, arg3);
		}

		private void Action<T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
		{
			this.ExecuteReflector(arg1, arg2, arg3, arg4);
		}

		private bool Predicate<T>(T arg)
		{
			return (bool)this.ExecuteReflector(arg);
		}

		private object ExecuteReflector(params object[] arguments)
		{
			object result = Execute(arguments);
			return result;
		}
	}
}