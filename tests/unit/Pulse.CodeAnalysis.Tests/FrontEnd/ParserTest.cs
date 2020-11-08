namespace Pulse.CodeAnalysis.Tests.FrontEnd
{
    using System;
    using System.Collections.Generic;
    using CodeAnalysis.FrontEnd;
    using CodeAnalysis.FrontEnd.Errors;
    using Moq;
    using Xunit;

    public class ParserTest
    {
        private readonly Mock<IErrorReporter> _errorReporterMock =
            new Mock<IErrorReporter>();

        [Theory]
        [MemberData(nameof(ParseTestCases))]
        internal void Parse_Returns_Expected_Expression(
            IEnumerable<Token> tokens,
            Action<Expression> inspector)
        {
            var parser = new Parser(
                _errorReporterMock.Object,
                tokens);
            var expression = parser.Parse();

            inspector(expression);
        }

        [Fact]
        internal void Parse_Reports_Error_When_Not_Expression()
        {
            // class + 1
            var tokens = new[]
            {
                new Token(
                    TokenType.Class,
                    Lexemes.Class,
                    null,
                    1),
                new Token(
                    TokenType.Plus,
                    Lexemes.Plus,
                    null,
                    1),
                new Token(
                    TokenType.Number,
                    "1",
                    1D,
                    1),
            };

            var parser = new Parser(
                _errorReporterMock.Object,
                tokens);
            var expression = parser.Parse();

            Assert.Null(expression);
            _errorReporterMock.Verify(
                x => x.ReportSyntaxError(It.IsAny<ParseException>()),
                Times.Once);
        }

        public static IEnumerable<object[]> ParseTestCases()
        {
            yield return NumberLiteralExpression();
            yield return StringLiteralExpression();
            yield return BooleanLiteralExpression();
            yield return UnaryExpression();
            yield return GroupingExpression();
            yield return EqualityExpression();
            yield return ComparisonExpression();
            yield return AdditionExpression();
        }

        private static object[] NumberLiteralExpression()
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
                    CreateEof(),
                },
                ExpressionAssertions.NumberInspector(2),
            };
        }

        private static object[] StringLiteralExpression()
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
                    CreateEof(),
                },
                ExpressionAssertions.StringInspector("pulse"),
            };
        }

        private static object[] BooleanLiteralExpression()
        {
            // "false"
            return new object[]
            {
                new[]
                {
                    new Token(
                        TokenType.False,
                        "false",
                        false,
                        1),
                    CreateEof(),
                },
                ExpressionAssertions.BooleanInspector(false),
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
                        Lexemes.Bang,
                        null,
                        1),
                    new Token(
                        TokenType.True,
                        Lexemes.True,
                        null,
                        1),
                    CreateEof(),
                },
                new Action<Expression>(
                    expression =>
                    {
                        var unary =
                            Assert.IsType<UnaryExpression>(expression);
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
                        Lexemes.LeftParen,
                        null,
                        1),
                    new Token(
                        TokenType.Number,
                        "1",
                        1D,
                        1),
                    new Token(
                        TokenType.Star,
                        Lexemes.Star,
                        null,
                        1),
                    new Token(
                        TokenType.Number,
                        "1",
                        1D,
                        1),
                    new Token(
                        TokenType.RightParen,
                        Lexemes.RightParen,
                        null,
                        1),
                    CreateEof(),
                },
                new Action<Expression>(
                    expression =>
                    {
                        var grouping =
                            Assert.IsType<GroupingExpression>(expression);
                        var binary =
                            Assert.IsType<BinaryExpression>(
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
                    CreateEof(),
                },
                new Action<Expression>(
                    expression =>
                    {
                        var equality =
                            Assert.IsType<BinaryExpression>(expression);
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

        private static object[] ComparisonExpression()
        {
            // 1 > 2
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
                        TokenType.Greater,
                        Lexemes.Greater,
                        null,
                        1),
                    new Token(
                        TokenType.Number,
                        "2",
                        2D,
                        1),
                    CreateEof(),
                },
                new Action<Expression>(
                    expression =>
                    {
                        var binary =
                            Assert.IsType<BinaryExpression>(expression);
                        Assert.Equal(
                            TokenType.Greater,
                            binary.Operator.Type);
                        ExpressionAssertions.Literal(
                            1D,
                            binary.Left);
                        ExpressionAssertions.Literal(
                            2D,
                            binary.Right);
                    }),
            };
        }

        private static object[] AdditionExpression()
        {
            // 1 + 2
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
                        TokenType.Plus,
                        Lexemes.Plus,
                        null,
                        1),
                    new Token(
                        TokenType.Number,
                        "2",
                        2D,
                        1),
                    CreateEof(),
                },
                new Action<Expression>(
                    expression =>
                    {
                        var binary =
                            Assert.IsType<BinaryExpression>(expression);
                        ExpressionAssertions.Literal(
                            1D,
                            binary.Left);
                        Assert.Equal(
                            TokenType.Plus,
                            binary.Operator.Type);
                        ExpressionAssertions.Literal(
                            2D,
                            binary.Right);
                    }),
            };
        }

        private static Token CreateEof()
            => new Token(
                TokenType.Eof,
                string.Empty,
                null,
                1);
    }
}
