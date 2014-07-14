namespace LightMock
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// A class that represents the mock context for a given <typeparamref name="TMock"/> type.
    /// </summary>
    /// <typeparam name="TMock">The target mock type.</typeparam>
    public class MockContext<TMock> : IMockContext<TMock>, IInvocationContext<TMock>
    {
        private readonly List<InvocationInfo> invocations = new List<InvocationInfo>();        
        private readonly List<ArrangementBase> arrangements = new List<ArrangementBase>(); 

        /// <summary>
        /// Verifies that the method represented by the <paramref name="matchExpression"/> has 
        /// been invoked.
        /// </summary>
        /// <param name="matchExpression">The <see cref="Expression{TDelegate}"/> that represents 
        /// the method invocation to be verified.</param>
        public void Assert(Expression<Action<TMock>> matchExpression)
        {
            Assert(matchExpression, Invoked.AtLeast(1));            
        }

        /// <summary>
        /// Verifies that the method represented by the <paramref name="matchExpression"/> has 
        /// been invoked the specified number of <paramref name="invoked"/>.
        /// </summary>
        /// <param name="matchExpression">The <see cref="Expression{TDelegate}"/> that represents 
        /// the method invocation to be verified.</param>
        /// <param name="invoked">Specifies the number of times we expect the mocked method to be invoked.</param>
        public void Assert(Expression<Action<TMock>> matchExpression, Invoked invoked)
        {
            var matchInfo = matchExpression.ToMatchInfo();

            var callCount = invocations.Count(matchInfo.Matches);
                        
            if (!invoked.Verify(callCount))
            {
                throw new InvalidOperationException(string.Format("The method {0} was called {1} times", matchExpression.Simplify(), callCount));
            }
        }

        /// <summary>
        /// Tracks that the method represented by the <paramref name="expression"/>
        /// has been invoked.
        /// </summary>
        /// <param name="expression">The <see cref="Expression{TDelegate}"/> that 
        /// represents the method that has been invoked.</param>
        void IInvocationContext<TMock>.Invoke(Expression<Action<TMock>> expression)
        {
            var invocationInfo = expression.ToInvocationInfo();
            invocations.Add(invocationInfo);
           
            var arrangement = arrangements.FirstOrDefault(a => a.Matches(invocationInfo));
            if (arrangement != null)
            {
                arrangement.Execute(invocationInfo.Arguments);
            }            
        }

        /// <summary>
        /// Tracks that the method represented by the <paramref name="expression"/>
        /// has been invoked.
        /// </summary>
        /// <typeparam name="TResult">The return type of the method that has been invoked.</typeparam>
        /// <param name="expression">The <see cref="Expression{TDelegate}"/> that 
        /// represents the method that has been invoked.</param>
        /// <returns>An instance of <typeparamref name="TResult"/> or possibly null 
        /// if <typeparamref name="TResult"/> a reference type.</returns>
        TResult IInvocationContext<TMock>.Invoke<TResult>(Expression<Func<TMock, TResult>> expression)
        {
            var invocationInfo = expression.ToInvocationInfo();
            invocations.Add(invocationInfo);

            var arrangement = arrangements.FirstOrDefault(a => a.Matches(invocationInfo));
            if (arrangement != null)
            {
                return (TResult)arrangement.Execute(invocationInfo.Arguments);
            }

            return default(TResult);
        }

        public Arrangement Arrange(Expression<Action<TMock>> expression)
        {
            var arrangement = new Arrangement(expression);
            arrangements.Add(arrangement);
            return arrangement;
        }

        public Arrangement<TResult> Arrange<TResult>(Expression<Func<TMock, TResult>> expression)
        {
            var arrangement = new Arrangement<TResult>(expression);
            arrangements.Add(arrangement);
            return arrangement;
        }        
    }



   


    public abstract class ArrangementBase
    {
        public LambdaExpression Expression { get; private set; }

        protected List<Action<object[]>> actions = new List<Action<object[]>>();

        public ArrangementBase(LambdaExpression expression)
        {
            Expression = expression;
        }

        public void Throws<TException>() where TException : Exception, new()
        {
            actions.Add((args) => { throw new TException(); });
        }

        public virtual object Execute(object[] arguments)
        {
            foreach (var action in actions)
            {
                action(arguments);
            }
            return null;
        }
        

        internal bool Matches(InvocationInfo invocationInfo)
        {
            return Expression.ToMatchInfo().Matches(invocationInfo);
        }
    }


    public class Arrangement : ArrangementBase
    {
        public Arrangement(LambdaExpression expression)
            : base(expression)
        {
        }

        public void Callback<T>(Action<T> callBack)
        {
            actions.Add(args => callBack.DynamicInvoke(args));
        }       
    }
    
    public class Arrangement<TResult> : ArrangementBase
    {
        private TResult result;
        
        public Arrangement(LambdaExpression expression)
            : base(expression)
        {
        }

        public Arrangement<TResult> Returns(TResult value)
        {
            result = value;
            return this;
        }

        public override object Execute(object[] arguments)
        {
            base.Execute(arguments);
            
            if (result.Equals(default(TResult)))
            {
                return default(TResult);
            }
            return result;
        }
    }


}