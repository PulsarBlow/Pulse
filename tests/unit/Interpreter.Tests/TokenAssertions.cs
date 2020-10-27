namespace Pulse.Interpreter.Tests
{
    using System;
    using Interpreter.FrontEnd;
    using Xunit;

    public static class TokenAssertions
    {
        public static void Eof(
            Token t)
            => Assert.True(
                t.Type == TokenType.Eof
                && t.Lexeme == string.Empty
                && t.Literal == null
                && t.Line == 1);

        public static void LeftParen(
            Token t)
            => Assert.True(
                t.Type == TokenType.LeftParen
                && t.Lexeme == Lexemes.LeftParen.ToString()
                && t.Literal == null
                && t.Line == 1);

        public static void RightParen(
            Token t)
            => Assert.True(
                t.Type == TokenType.RightParen
                && t.Lexeme == Lexemes.RightParen.ToString()
                && t.Literal == null
                && t.Line == 1);

        public static void LeftBrace(
            Token t)
            => Assert.True(
                t.Type == TokenType.LeftBrace
                && t.Lexeme == Lexemes.LeftBrace.ToString()
                && t.Literal == null
                && t.Line == 1);

        public static void RightBrace(
            Token t)
            => Assert.True(
                t.Type == TokenType.RightBrace
                && t.Lexeme == Lexemes.RightBrace.ToString()
                && t.Literal == null
                && t.Line == 1);

        public static void Comma(
            Token t)
            => Assert.True(
                t.Type == TokenType.Comma
                && t.Lexeme == Lexemes.Comma.ToString()
                && t.Literal == null
                && t.Line == 1);

        public static void Dot(
            Token t)
            => Assert.True(
                t.Type == TokenType.Dot
                && t.Lexeme == Lexemes.Dot.ToString()
                && t.Literal == null
                && t.Line == 1);

        public static void Minus(
            Token t)
            => Assert.True(
                t.Type == TokenType.Minus
                && t.Lexeme == Lexemes.Minus.ToString()
                && t.Literal == null
                && t.Line == 1);

        public static void Plus(
            Token t)
            => Assert.True(
                t.Type == TokenType.Plus
                && t.Lexeme == Lexemes.Plus.ToString()
                && t.Literal == null
                && t.Line == 1);

        public static void Semicolon(
            Token t)
            => Assert.True(
                t.Type == TokenType.Semicolon
                && t.Lexeme == Lexemes.Semicolon.ToString()
                && t.Literal == null
                && t.Line == 1);

        public static void Star(
            Token t)
            => Assert.True(
                t.Type == TokenType.Star
                && t.Lexeme == Lexemes.Star.ToString()
                && t.Literal == null
                && t.Line == 1);

        public static void Bang(
            Token t)
            => Assert.True(
                t.Type == TokenType.Bang
                && t.Lexeme == Lexemes.Bang.ToString()
                && t.Literal == null
                && t.Line == 1);

        public static void BangEqual(
            Token t)
            => Assert.True(
                t.Type == TokenType.BangEqual
                && t.Lexeme == Lexemes.BangEqual
                && t.Literal == null
                && t.Line == 1);

        public static void Equal(
            Token t)
            => Assert.True(
                t.Type == TokenType.Equal
                && t.Lexeme == Lexemes.Equal.ToString()
                && t.Literal == null
                && t.Line == 1);

        public static Action<Token> Equal(
            object literal = null,
            int line = 1)
            => t => Assert.True(
                t.Type == TokenType.Equal
                && t.Lexeme == Lexemes.Equal.ToString()
                && t.Literal == literal
                && t.Line == line);

        public static void EqualEqual(
            Token t)
            => Assert.True(
                t.Type == TokenType.EqualEqual
                && t.Lexeme == Lexemes.EqualEqual
                && t.Literal == null
                && t.Line == 1);

        public static void Less(
            Token t)
            => Assert.True(
                t.Type == TokenType.Less
                && t.Lexeme == Lexemes.Less.ToString()
                && t.Literal == null
                && t.Line == 1);

        public static void LessEqual(
            Token t)
            => Assert.True(
                t.Type == TokenType.LessEqual
                && t.Lexeme == Lexemes.LessEqual
                && t.Literal == null
                && t.Line == 1);

        public static void Greater(
            Token t)
            => Assert.True(
                t.Type == TokenType.Greater
                && t.Lexeme == Lexemes.Greater.ToString()
                && t.Literal == null
                && t.Line == 1);

        public static void GreaterEqual(
            Token t)
            => Assert.True(
                t.Type == TokenType.GreaterEqual
                && t.Lexeme == Lexemes.GreaterEqual
                && t.Literal == null
                && t.Line == 1);

        public static void Slash(
            Token t)
            => Assert.True(
                t.Type == TokenType.Slash
                && t.Lexeme == Lexemes.Slash.ToString()
                && t.Literal == null
                && t.Line == 1);

        public static void Var(
            Token t)
            => Assert.True(
                t.Type == TokenType.Var
                && t.Lexeme == Lexemes.Var
                && t.Literal == null
                && t.Line == 1);

        public static Action<Token> Token(
            TokenType tokenType,
            char lexeme,
            int line,
            object literal = null)
            => Token(
                tokenType,
                lexeme.ToString(),
                line,
                literal);

        public static Action<Token> Token(
            TokenType tokenType,
            string lexeme,
            int line,
            object literal = null)
        {
            return t => Assert.True(
                t.Type == tokenType
                && t.Lexeme == lexeme
                && t.Literal == literal
                && t.Line == line);
        }

        public static Action<Token> Identifier(
            string lexeme,
            int line = 1)
        {
            return t => Assert.True(
                t.Type == TokenType.Identifier
                && t.Lexeme == lexeme
                && t.Literal == null
                && t.Line == line);
        }

        public static Action<Token> Number(
            string lexeme,
            double literal,
            int line = 1)
        {
            const double roundingSafeGuard = 0.001;
            return t => Assert.True(
                t.Literal != null
                && t.Type == TokenType.Number
                && t.Lexeme == lexeme
                // Rounding safe-guard comparison
                && Math.Abs((double) t.Literal - literal) < roundingSafeGuard
                && t.Line == line);
        }

        public static Action<Token> String(
            string lexeme,
            string literal,
            int line = 1)
        {
            return t =>
            {
                Assert.True(
                    t.Literal != null
                    && t.Type == TokenType.String
                    && t.Lexeme == lexeme
                    && (string) t.Literal == literal
                    && t.Line == line);
            };
        }
    }
}
