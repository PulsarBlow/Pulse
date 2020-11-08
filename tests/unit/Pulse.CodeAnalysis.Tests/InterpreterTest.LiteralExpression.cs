namespace Pulse.CodeAnalysis.Tests
{
    using System.Collections.Generic;
    using CodeAnalysis.FrontEnd;
    using Xunit;

    // Tests for LiteralExpression
    public partial class InterpreterTest
    {
        [Theory]
        [MemberData(nameof(VisitLiteralExpression_Valid_TestCases))]
        internal void VisitLiteralExpression_Returns_Expected_Result(
            LiteralExpression expression,
            object expected)
        {
            var interpreter = CreateInterpreter();
            var result =
                interpreter.VisitLiteralExpression(expression);

            Assert.Equal(
                expected,
                result);
        }

        public static IEnumerable<object[]>
            VisitLiteralExpression_Valid_TestCases()
        {
            yield return new object[]
            {
                new LiteralExpression(1D),
                1D,
            };

            yield return new object[]
            {
                new LiteralExpression(true),
                true,
            };

            yield return new object[]
            {
                new LiteralExpression(false),
                false,
            };

            yield return new object[]
            {
                new LiteralExpression("value"),
                "value",
            };
        }
    }
}
