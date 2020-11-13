namespace Pulse.CodeAnalysis.Tests
{
    using System.Collections.Generic;
    using CodeAnalysis.FrontEnd;
    using Xunit;

    // Tests for GroupingExpression
    public partial class InterpreterTest
    {
        [Theory]
        [MemberData(nameof(VisitGroupingExpression_Valid_TestCases))]
        internal void VisitGroupingExpression_Returns_Expected_Result(
            GroupingExpression expression,
            object expected)
        {
            var interpreter = CreateInterpreter();
            var result =
                interpreter.VisitGroupingExpression(expression);

            Assert.Equal(
                expected,
                result);
        }

        public static IEnumerable<object[]>
            VisitGroupingExpression_Valid_TestCases()
        {
            // (1 + 1) -> 2
            yield return new object[]
            {
                new GroupingExpression(
                    new BinaryExpression(
                        new LiteralExpression(1D),
                        CreateOperator(
                            TokenType.Plus,
                            Lexemes.Plus),
                        new LiteralExpression(1D))),
                2D,
            };

            // ((1 + 1) * 2) -> 4
            yield return new object[]
            {
                new GroupingExpression(
                    new BinaryExpression(
                        new GroupingExpression(
                            new BinaryExpression(
                                new LiteralExpression(1D),
                                CreateOperator(
                                    TokenType.Plus,
                                    Lexemes.Plus),
                                new LiteralExpression(1D))),
                        CreateOperator(
                            TokenType.Star,
                            Lexemes.Star),
                        new LiteralExpression(2D))),
                4D,
            };

            // ((1 * 2) + 1) -> 3
            yield return new object[]
            {
                new GroupingExpression(
                    new BinaryExpression(
                        new GroupingExpression(
                            new BinaryExpression(
                                new LiteralExpression(1D),
                                CreateOperator(
                                    TokenType.Star,
                                    Lexemes.Star),
                                new LiteralExpression(2D))),
                        CreateOperator(
                            TokenType.Plus,
                            Lexemes.Plus),
                        new LiteralExpression(1D))),
                3D,
            };

            // ((1 + 1) / (1 * 1)) -> 2
            yield return new object[]
            {
                new GroupingExpression(
                    new BinaryExpression(
                        new GroupingExpression(
                            new BinaryExpression(
                                new LiteralExpression(1D),
                                CreateOperator(
                                    TokenType.Plus,
                                    Lexemes.Plus),
                                new LiteralExpression(1D))),
                        CreateOperator(
                            TokenType.Slash,
                            Lexemes.Slash),
                        new GroupingExpression(
                            new BinaryExpression(
                                new LiteralExpression(1D),
                                CreateOperator(
                                    TokenType.Star,
                                    Lexemes.Star),
                                new LiteralExpression(1D))))),
                2D,
            };
        }
    }
}
