using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace LightMock
{
    internal class LambdaComparer : IEqualityComparer<Expression>
    {
        static LambdaComparer instance;
        public static LambdaComparer Instance => LazyInitializer.EnsureInitialized(ref instance);

        public bool Equals(Expression x, Expression y)
        {
            if (object.ReferenceEquals(x, y))
                return true;
            if (x == null || y == null)
                return false;
            if (x.NodeType == y.NodeType && x.Type == y.Type)
            {
                switch (x)
                {
                    case LambdaExpression lambda:
                        return EqualsLambda(lambda, y);
                    case MethodCallExpression methodCall:
                        return EqualsMethodCall(methodCall, y);
                    case ParameterExpression _:
                        return true;
                    case ConstantExpression constant:
                        return EqualsConstant(constant, y);
                    case MemberExpression member:
                        return EqualsMember(member, y);
                    case BinaryExpression binary:
                        return EqualsBinary(binary, y);
                }
                throw new InvalidOperationException($"expression type {x.NodeType}/{x.GetType()} is not supported");
            }
            return false;
        }

        private bool EqualsBinary(BinaryExpression left, Expression y)
        {
            var right = (BinaryExpression)y;
            return left.Method == right.Method
                && Equals(left.Left, right.Left)
                && Equals(left.Right, right.Right)
                && Equals(left.Conversion, right.Conversion);
        }

        private bool EqualsMember(MemberExpression left, Expression y)
        {
            var right = (MemberExpression)y;
            return left.Member == right.Member
                && Equals(left.Expression, right.Expression);
        }

        private bool EqualsConstant(ConstantExpression left, Expression y)
        {
            var right = (ConstantExpression)y;
            return object.Equals(left.Value, right.Value);
        }

        private bool EqualsMethodCall(MethodCallExpression left, Expression y)
        {
            var right = (MethodCallExpression)y;
            return left.Method == right.Method
                && Equals(left.Object, right.Object)
                && EqualsParameters(left.Arguments, right.Arguments);
        }

        private bool EqualsLambda(LambdaExpression left, Expression y)
        {
            var right = (LambdaExpression)y;
            return Equals(left.Body, right.Body) && EqualsParameters(left.Parameters, right.Parameters);
        }

        private bool EqualsParameters<T>(ReadOnlyCollection<T> left, ReadOnlyCollection<T> right)
            where T : Expression
        {
            return left.Count == right.Count && Enumerable.SequenceEqual(left, right, this);
        }

        public int GetHashCode(Expression obj) => obj?.GetHashCode() ?? 0;

    }
}
