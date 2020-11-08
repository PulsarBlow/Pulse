namespace Pulse.CodeAnalysis.Tests.FrontEnd
{
    using System;
    using System.Collections.Generic;
    using CodeAnalysis.FrontEnd;
    using CodeAnalysis.FrontEnd.Errors;
    using Moq;
    using Xunit;

    public class LexerTest
    {
        [Theory]
        [MemberData(nameof(ReadToken_TestCases))]
        public void ReadToken_Reads_Expected_Tokens(
            string source,
            Action<Token>[] inspectors)
        {
            var lexer = new Lexer(
                source,
                Mock.Of<IErrorReporter>());
            var tokens = lexer.ReadTokens();
            Assert.Collection(
                tokens,
                inspectors);
        }

        [Theory]
        [InlineData("%")]
        [InlineData("^")]
        [InlineData("£")]
        public void ReadToken_Reports_Error_When_Unexpected_Character(
            string source)
        {
            var errorReporter = new Mock<IErrorReporter>();
            var lexer = new Lexer(
                source,
                errorReporter.Object);
            lexer.ReadTokens();

            errorReporter.Verify(
                x => x.ReportSyntaxError(
                    It.Is<int>(i => i == 1),
                    It.Is<string>(
                        s => s.Contains(
                            "unexpected",
                            StringComparison.OrdinalIgnoreCase))));
        }

        [Fact]
        public void ReadToken_Reports_Error_When_Unterminated_String()
        {
            var errorReporter = new Mock<IErrorReporter>();
            var lexer = new Lexer(
                "\"word",
                errorReporter.Object);
            lexer.ReadTokens();

            errorReporter.Verify(
                x => x.ReportSyntaxError(
                    It.Is<int>(i => i == 1),
                    It.Is<string>(
                        s => s.Contains(
                            "unterminated",
                            StringComparison.OrdinalIgnoreCase))),
                Times.Once());
        }

        public static IEnumerable<object[]> ReadToken_TestCases()
        {
            yield return CreateInspector(
                Lexemes.LeftParen,
                TokenAssertions.LeftParen);

            yield return CreateInspector(
                Lexemes.RightParen,
                TokenAssertions.RightParen);

            yield return CreateInspector(
                Lexemes.LeftBrace,
                TokenAssertions.LeftBrace);

            yield return CreateInspector(
                Lexemes.RightBrace,
                TokenAssertions.RightBrace);

            yield return CreateInspector(
                Lexemes.Comma,
                TokenAssertions.Comma);

            yield return CreateInspector(
                Lexemes.Dot,
                TokenAssertions.Dot);

            yield return CreateInspector(
                Lexemes.Minus,
                TokenAssertions.Minus);

            yield return CreateInspector(
                Lexemes.Plus,
                TokenAssertions.Plus);

            yield return CreateInspector(
                Lexemes.Semicolon,
                TokenAssertions.Semicolon);

            yield return CreateInspector(
                Lexemes.Star,
                TokenAssertions.Star);

            yield return CreateInspector(
                Lexemes.Bang,
                TokenAssertions.Bang);

            yield return CreateInspector(
                Lexemes.BangEqual,
                TokenAssertions.BangEqual);

            yield return CreateInspector(
                Lexemes.Equal,
                TokenAssertions.Equal);

            yield return CreateInspector(
                Lexemes.EqualEqual,
                TokenAssertions.EqualEqual);

            yield return CreateInspector(
                Lexemes.Less,
                TokenAssertions.Less);

            yield return CreateInspector(
                Lexemes.LessEqual,
                TokenAssertions.LessEqual);

            yield return CreateInspector(
                Lexemes.Greater,
                TokenAssertions.Greater);

            yield return CreateInspector(
                Lexemes.GreaterEqual,
                TokenAssertions.GreaterEqual);

            yield return CreateInspector(
                Lexemes.Slash,
                TokenAssertions.Slash);

            yield return new object[]
            {
                "// A comment",
                new Action<Token>[] { TokenAssertions.Eof },
            };

            yield return new object[]
            {
                "1.123",
                new[]
                {
                    TokenAssertions.Number(
                        "1.123",
                        1.123D),
                    TokenAssertions.Eof,
                },
            };

            yield return new object[]
            {
                "\"word\"",
                new[]
                {
                    TokenAssertions.String(
                        "\"word\"",
                        "word"),
                    TokenAssertions.Eof,
                },
            };

            yield return new object[]
            {
                "Identifier",
                new[]
                {
                    TokenAssertions.Identifier("Identifier"),
                    TokenAssertions.Eof,
                },
            };

            yield return new object[]
            {
                "var a = 1",
                new[]
                {
                    TokenAssertions.Var,
                    TokenAssertions.Identifier("a"),
                    TokenAssertions.Equal,
                    TokenAssertions.Number(
                        "1",
                        1D),
                    TokenAssertions.Eof,
                },
            };

            yield return new object[]
            {
                @"class Animal {
                    this.genus = ""Acinonyx"";
                  }",
                new[]
                {
                    TokenAssertions.Token(
                        TokenType.Class,
                        Lexemes.Class,
                        1),
                    TokenAssertions.Identifier("Animal"),
                    TokenAssertions.Token(
                        TokenType.LeftBrace,
                        Lexemes.LeftBrace,
                        1),
                    TokenAssertions.Token(
                        TokenType.This,
                        Lexemes.This,
                        2),
                    TokenAssertions.Token(
                        TokenType.Dot,
                        Lexemes.Dot,
                        2),
                    TokenAssertions.Identifier(
                        "genus",
                        2),
                    TokenAssertions.Equal(line: 2),
                    TokenAssertions.String(
                        "\"Acinonyx\"",
                        "Acinonyx",
                        2),
                    TokenAssertions.Token(
                        TokenType.Semicolon,
                        Lexemes.Semicolon,
                        2),
                    TokenAssertions.Token(
                        TokenType.RightBrace,
                        Lexemes.RightBrace,
                        3),
                    TokenAssertions.Token(
                        TokenType.Eof,
                        string.Empty,
                        3),
                },
            };
        }

        private static object[] CreateInspector(
            char lexeme,
            params Action<Token>[] actions)
            => CreateInspector(
                lexeme.ToString(),
                actions);

        private static object[] CreateInspector(
            string lexeme,
            params Action<Token>[] actions)
        {
            return new object[]
            {
                lexeme,
                WrapInspectors(actions),
            };
        }

        private static Action<Token>[] WrapInspectors(
            params Action<Token>[] actions)
        {
            var inspectors = new List<Action<Token>>(actions)
            {
                TokenAssertions.Eof,
            };
            return inspectors.ToArray();
        }
    }
}
