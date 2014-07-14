namespace LightMock
{
    using System.Linq.Expressions;

    /// <summary>
    /// Represents a class that is capable of building 
    /// a set of lambda expressions used to match argument values.
    /// </summary>
    internal interface IMatchInfoBuilder
    {
        /// <summary>
        /// Builds a <see cref="LambdaExpression"/> for each 
        /// argument in the method call where each <see cref="LambdaExpression"/>
        /// represents matching an argument value.
        /// </summary>
        /// <param name="expression">The target <see cref="LambdaExpression"/>.</param>
        /// <returns>An array where each element is a <see cref="LambdaExpression"/> 
        /// representing matching an argument value.</returns>
        MatchInfo Build(LambdaExpression expression);
    }
}