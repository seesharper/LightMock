namespace LightMock
{
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using ExpressionReflect;

    internal class MatchInfo
    {
        public MatchInfo(MethodInfo method, LambdaExpression[] matchExpressions)
        {
            Method = method;
            MatchExpressions = matchExpressions;
        }

        public MethodInfo Method { get; private set; }

        public LambdaExpression[] MatchExpressions { get; private set; }

        public bool Matches(InvocationInfo invocationInfo)
        {            
            if (Method != invocationInfo.Method)
            {
                return false;
            }

            if (MatchExpressions.Length != invocationInfo.Arguments.Length)
            {
                return false;
            }

            return !MatchExpressions.Where((t, i) => !(bool)t.Execute(invocationInfo.Arguments[i])).Any();
        }
    }
}