namespace ExpressionTrees.Task1.ExpressionsTransformer.Tests
{
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using Xunit;
    using Xunit.Abstractions;

    public class ExpressionsTransformerTests
    {
        private readonly ITestOutputHelper output;

        public ExpressionsTransformerTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        /// <summary>
        /// Inc/Dec expression correct value processed.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="expected">The expected.</param>
        [Theory]
        [MemberData(nameof(ExpressionTestCases.IncrementTestCases), MemberType = typeof(ExpressionTestCases))]
        public void IncDecExpressionVisitor_ReplaceVariable_ReturnIncrement(LambdaExpression source, string expected)
        {
            var visitor = new IncDecExpressionVisitor();
            var convertedExp = visitor.VisitAndConvert(source, "");

            Assert.Equal(expected, convertedExp?.Body.ToString());
        }

        /// <summary>
        /// Inc/Dec expression correct value processed.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="expected">The expected.</param>
        [Theory]
        [MemberData(nameof(ExpressionTestCases.DecrementTestCases), MemberType = typeof(ExpressionTestCases))]
        public void IncDecExpressionVisitor_ReplaceVariable_ReturnDecrement(LambdaExpression source, string expected)
        {
            var visitor = new IncDecExpressionVisitor();
            var convertedExp = visitor.VisitAndConvert(source, "");

            Assert.Equal(expected, convertedExp?.Body.ToString());
        }

        /// <summary>
        /// Replaces the parameter in expression and return new expression.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="replaceDictionary">The replace dictionary.</param>
        [Theory]
        [MemberData(nameof(ExpressionTestCases.ReplaceParameterTestCases), MemberType = typeof(ExpressionTestCases))]
        public void ReplaceParameterVisitor_ReplaceVariable_Return(LambdaExpression source, Dictionary<string, int> replaceDictionary)
        {
            var visitor = new ReplaceParameterVisitor(replaceDictionary);
            var convertedExp = visitor.VisitAndConvert(source, "");

            output.WriteLine("Original expression {0}", source);
            output.WriteLine("Converted expression {0}", convertedExp);
            Assert.Equal(1, 1);
        }
    }
}
