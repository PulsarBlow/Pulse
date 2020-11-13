namespace Pulse.CodeAnalysis.Tests
{
    using System.Collections.Generic;
    using CodeAnalysis.FrontEnd;
    using CodeAnalysis.FrontEnd.Errors;
    using Xunit;

    // Tests for BinaryExpression
    public partial class InterpreterTest
    {
        [Theory]
        [MemberData(nameof(VisitBinaryExpression_Valid_TestCases))]
        internal void VisitBinaryExpression_Returns_Expected_Result(
            Expression left,
            Token op,
            Expression right,
            object expected)
        {
            var expression = new BinaryExpression(
                left,
                op,
                right);
            var interpreter = CreateInterpreter();
            var result = interpreter.VisitBinaryExpression(expression);

            Assert.Equal(
                expected,
                result);
        }

        [Theory]
        [MemberData(nameof(VisitBinaryExpression_Error_TestCases))]
        internal void VisitBinaryExpression_Throws_RuntimeException(
            Expression left,
            Token op,
            Expression right)
        {
            var expression = new BinaryExpression(
                left,
                op,
                right);
            var interpreter = CreateInterpreter();

            Assert.Throws<RuntimeException>(
                () => interpreter.VisitBinaryExpression(expression));
        }

        public static IEnumerable<object[]>
            VisitBinaryExpression_Valid_TestCases()
        {
            // 1 > 0 -> True
            yield return new object[]
            {
                new LiteralExpression(1D),
                CreateOperator(
                    TokenType.Greater,
                    Lexemes.Greater),
                new LiteralExpression(0D),
                true,
            };

            // 1 >= 0 -> True
            yield return new object[]
            {
                new LiteralExpression(1D),
                CreateOperator(
                    TokenType.GreaterEqual,
                    Lexemes.GreaterEqual),
                new LiteralExpression(0D),
                true,
            };

            // 1 >= 1 -> True
            yield return new object[]
            {
                new LiteralExpression(1D),
                CreateOperator(
                    TokenType.GreaterEqual,
                    Lexemes.GreaterEqual),
                new LiteralExpression(1D),
                true,
            };

            // 1 < 2 -> True
            yield return new object[]
            {
                new LiteralExpression(1D),
                CreateOperator(
                    TokenType.Less,
                    Lexemes.LeftParen),
                new LiteralExpression(2D),
                true,
            };

            // 1 <= 2 -> True
            yield return new object[]
            {
                new LiteralExpression(1D),
                CreateOperator(
                    TokenType.LessEqual,
                    Lexemes.LessEqual),
                new LiteralExpression(2D),
                true,
            };

            // 1 - 1 -> 0
            yield return new object[]
            {
                new LiteralExpression(1D),
                CreateOperator(
                    TokenType.Minus,
                    Lexemes.Minus),
                new LiteralExpression(1D),
                0D,
            };

            // 1 + 1 -> 2
            yield return new object[]
            {
                new LiteralExpression(1D),
                CreateOperator(
                    TokenType.Plus,
                    Lexemes.Plus),
                new LiteralExpression(1D),
                2D,
            };

            // 1 / 1 -> 1
            yield return new object[]
            {
                new LiteralExpression(1D),
                CreateOperator(
                    TokenType.Slash,
                    Lexemes.Star),
                new LiteralExpression(1D),
                1D,
            };

            // 1 * 1 -> 1
            yield return new object[]
            {
                new LiteralExpression(1D),
                CreateOperator(
                    TokenType.Star,
                    Lexemes.Star),
                new LiteralExpression(1D),
                1D,
            };

            // 1 != 2 -> True
            yield return new object[]
            {
                new LiteralExpression(1D),
                CreateOperator(
                    TokenType.BangEqual,
                    Lexemes.BangEqual),
                new LiteralExpression(2D),
                true,
            };

            // null != 1 -> True
            yield return new object[]
            {
                new LiteralExpression(null),
                CreateOperator(
                    TokenType.BangEqual,
                    Lexemes.BangEqual),
                new LiteralExpression(1D),
                true,
            };

            // 1 == 1 -> True
            yield return new object[]
            {
                new LiteralExpression(1D),
                CreateOperator(
                    TokenType.EqualEqual,
                    Lexemes.EqualEqual),
                new LiteralExpression(1D),
                true,
            };

            // nil == nil -> True
            yield return new object[]
            {
                new LiteralExpression(null),
                CreateOperator(
                    TokenType.EqualEqual,
                    Lexemes.EqualEqual),
                new LiteralExpression(null),
                true,
            };

            // 1 == 2 -> False
            yield return new object[]
            {
                new LiteralExpression(1D),
                CreateOperator(
                    TokenType.EqualEqual,
                    Lexemes.EqualEqual),
                new LiteralExpression(2D),
                false,
            };

            // true == 1 -> False
            yield return new object[]
            {
                new LiteralExpression(true),
                CreateOperator(
                    TokenType.EqualEqual,
                    Lexemes.EqualEqual),
                new LiteralExpression(1D),
                false,
            };

            // true != 1 -> True
            yield return new object[]
            {
                new LiteralExpression(true),
                CreateOperator(
                    TokenType.BangEqual,
                    Lexemes.BangEqual),
                new LiteralExpression(1D),
                true,
            };

            // "a" + "b" -> "ab"
            yield return new object[]
            {
                new LiteralExpression("a"),
                CreateOperator(
                    TokenType.Plus,
                    Lexemes.Plus),
                new LiteralExpression("b"),
                "ab",
            };

            // "a" + "" -> "a"
            yield return new object[]
            {
                new LiteralExpression("a"),
                CreateOperator(
                    TokenType.Plus,
                    Lexemes.Plus),
                new LiteralExpression(string.Empty),
                "a",
            };
        }

        public static IEnumerable<object[]>
            VisitBinaryExpression_Error_TestCases()
        {
            // true / 1
            yield return new object[]
            {
                new LiteralExpression(true),
                CreateOperator(
                    TokenType.Slash,
                    Lexemes.Slash),
                new LiteralExpression(1D),
            };

            // a + 1
            yield return new object[]
            {
                new LiteralExpression("a"),
                CreateOperator(
                    TokenType.Plus,
                    Lexemes.Plus),
                new LiteralExpression(1D),
            };

            // a + true
            yield return new object[]
            {
                new LiteralExpression("a"),
                CreateOperator(
                    TokenType.Plus,
                    Lexemes.Plus),
                new LiteralExpression(true),
            };

            // "a" + nil
            yield return new object[]
            {
                new LiteralExpression("a"),
                CreateOperator(
                    TokenType.Plus,
                    Lexemes.Plus),
                new LiteralExpression(null),
            };
        }
    }
}
