namespace Pulse.Interpreter.Tests.FrontEnd
{
    using System;
    using System.Collections.Generic;
    using Interpreter.FrontEnd;
    using Xunit;

    public class ParserTest
    {
        [Theory]
        [MemberData(nameof(ParseTestCases))]
        internal void Parse_Returns_Expected_Expression(
            IEnumerable<Token> tokens,
            Action<Expression> inspector)
        {
            var parser = new Parser(tokens);
            var expression = parser.Parse();

            inspector(expression);
        }

        public static IEnumerable<object[]> ParseTestCases()
        {
            yield return NumberExpression();
            yield return StringExpression();
            yield return UnaryExpression();
            yield return GroupingExpression();
            yield return EqualityExpression();
        }

        private static object[] NumberExpression()
        {
            // 2
            return new object[]
            {
                new[]
                {
                    new Token(
                        TokenType.Number,
                        "2",
                        2D,
                        1),
                    new Token(
                        TokenType.Eof,
                        string.Empty,
                        null,
                        1),
                },
                ExpressionAssertions.NumberInspector(2),
            };
        }

        private static object[] StringExpression()
        {
            // "pulse"
            return new object[]
            {
                new[]
                {
                    new Token(
                        TokenType.String,
                        "pulse",
                        "pulse",
                        1),
                    new Token(
                        TokenType.Eof,
                        string.Empty,
                        null,
                        1),
                },
                ExpressionAssertions.StringInspector("pulse"),
            };
        }

        private static object[] UnaryExpression()
        {
            // !true
            return new object[]
            {
                new[]
                {
                    new Token(
                        TokenType.Bang,
                        Lexemes.Bang.ToString(),
                        null,
                        1),
                    new Token(
                        TokenType.True,
                        Lexemes.True,
                        null,
                        1),
                    new Token(
                        TokenType.Eof,
                        string.Empty,
                        null,
                        1),
                },
                new Action<Expression>(
                    expression =>
                    {
                        var unary =
                            Assert.IsType<Expression.Unary>(expression);
                        Assert.Equal(
                            TokenType.Bang,
                            unary.Operator.Type);
                        ExpressionAssertions.Literal(
                            true,
                            unary.Right);
                    }),
            };
        }

        private static object[] GroupingExpression()
        {
            // (1 * 1)
            return new object[]
            {
                new[]
                {
                    new Token(
                        TokenType.LeftParen,
                        Lexemes.LeftParen.ToString(),
                        null,
                        1),
                    new Token(
                        TokenType.Number,
                        "1",
                        1D,
                        1),
                    new Token(
                        TokenType.Star,
                        Lexemes.Star.ToString(),
                        null,
                        1),
                    new Token(
                        TokenType.Number,
                        "1",
                        1D,
                        1),
                    new Token(
                        TokenType.RightParen,
                        Lexemes.RightParen.ToString(),
                        null,
                        1),
                    new Token(
                        TokenType.Eof,
                        string.Empty,
                        null,
                        1),
                },
                new Action<Expression>(
                    expression =>
                    {
                        var grouping =
                            Assert.IsType<Expression.Grouping>(expression);
                        var binary =
                            Assert.IsType<Expression.Binary>(
                                grouping.Expression);
                        ExpressionAssertions.Literal(
                            1,
                            binary.Left);
                        ExpressionAssertions.Literal(
                            1,
                            binary.Right);
                        Assert.Equal(
                            TokenType.Star,
                            binary.Operator.Type);
                    }),
            };
        }

        private static object[] EqualityExpression()
        {
            // 1 == 1
            return new object[]
            {
                new[]
                {
                    new Token(
                        TokenType.Number,
                        "1",
                        1D,
                        1),
                    new Token(
                        TokenType.EqualEqual,
                        Lexemes.EqualEqual,
                        null,
                        1),
                    new Token(
                        TokenType.Number,
                        "1",
                        1D,
                        1),
                    new Token(
                        TokenType.Eof,
                        string.Empty,
                        null,
                        1),
                },
                new Action<Expression>(
                    expression =>
                    {
                        var equality =
                            Assert.IsType<Expression.Binary>(expression);
                        ExpressionAssertions.Literal(
                            1,
                            equality.Left);
                        ExpressionAssertions.Literal(
                            1,
                            equality.Right);
                        Assert.Equal(
                            TokenType.EqualEqual,
                            equality.Operator.Type);
                    }),
            };
        }
    }
}
