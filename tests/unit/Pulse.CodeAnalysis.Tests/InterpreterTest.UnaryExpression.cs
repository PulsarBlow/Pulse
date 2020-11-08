namespace Pulse.CodeAnalysis.Tests
{
    using System.Collections.Generic;
    using CodeAnalysis.FrontEnd;
    using CodeAnalysis.FrontEnd.Errors;
    using Xunit;

    // Tests for UnaryExpression
    public partial class InterpreterTest
    {
        [Theory]
        [MemberData(nameof(VisitUnaryExpression_Valid_TestCases))]
        internal void VisitUnaryExpression_Returns_Expected_Result(
            UnaryExpression expression,
            object expected)
        {
            var interpreter = CreateInterpreter();
            var result = interpreter.VisitUnaryExpression(expression);

            Assert.Equal(
                expected,
                result);
        }

        [Theory]
        [MemberData(nameof(VisitUnaryExpression_Error_TestCases))]
        internal void VisitUnaryExpression_Throws_RuntimeException(
            UnaryExpression expression)
        {
            var interpreter = CreateInterpreter();

            Assert.Throws<RuntimeException>(
                () => interpreter.VisitUnaryExpression(expression));
        }

        public static IEnumerable<object[]>
            VisitUnaryExpression_Valid_TestCases()
        {
            // !true -> false
            yield return new object[]
            {
                new UnaryExpression(
                    CreateOperator(
                        TokenType.Bang,
                        Lexemes.Bang),
                    new LiteralExpression(true)),
                false,
            };

            // !!false -> false
            yield return new object[]
            {
                new UnaryExpression(
                    CreateOperator(
                        TokenType.Bang,
                        Lexemes.Bang),
                    new UnaryExpression(
                        CreateOperator(
                            TokenType.Bang,
                            Lexemes.Bang),
                        new LiteralExpression(false))),
                false,
            };

            // -1 -> -1
            yield return new object[]
            {
                new UnaryExpression(
                    CreateOperator(
                        TokenType.Minus,
                        Lexemes.Minus),
                    new LiteralExpression(1D)),
                -1D,
            };

            // --1 -> 1
            yield return new object[]
            {
                new UnaryExpression(
                    CreateOperator(
                        TokenType.Minus,
                        Lexemes.Minus),
                    new UnaryExpression(
                        CreateOperator(
                            TokenType.Minus,
                            Lexemes.Minus),
                        new LiteralExpression(1D))),
                1D,
            };

            // !1 -> False
            yield return new object[]
            {
                new UnaryExpression(
                    CreateOperator(
                        TokenType.Bang,
                        Lexemes.Bang),
                    new LiteralExpression(1D)),
                false,
            };

            // !"a" -> False
            yield return new object[]
            {
                new UnaryExpression(
                    CreateOperator(
                        TokenType.Bang,
                        Lexemes.Bang),
                    new LiteralExpression("a")),
                false,
            };

            // !null -> True
            yield return new object[]
            {
                new UnaryExpression(
                    CreateOperator(
                        TokenType.Bang,
                        Lexemes.Bang),
                    new LiteralExpression(null)),
                true,
            };
        }

        public static IEnumerable<object[]>
            VisitUnaryExpression_Error_TestCases()
        {
            // -true -> Error
            yield return new object[]
            {
                new UnaryExpression(
                    CreateOperator(
                        TokenType.Minus,
                        Lexemes.Minus),
                    new LiteralExpression(true)),
            };

            // -"a" -> Error
            yield return new object[]
            {
                new UnaryExpression(
                    CreateOperator(
                        TokenType.Minus,
                        Lexemes.Minus),
                    new LiteralExpression("a")),
            };
        }
    }
}
