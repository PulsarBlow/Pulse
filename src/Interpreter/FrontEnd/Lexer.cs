namespace Pulse.Interpreter.FrontEnd
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public class Lexer
    {
        private int _start;
        private int _current;
        private int _line = 1;
        private readonly string _source;
        private readonly List<Token> _tokens = new List<Token>();
        private readonly Dictionary<string, TokenType> _keywords =
            new Dictionary<string, TokenType>
            {
                [Lexemes.And] = TokenType.And,
                [Lexemes.Class] = TokenType.Class,
                [Lexemes.Else] = TokenType.Else,
                [Lexemes.False] = TokenType.False,
                [Lexemes.For] = TokenType.For,
                [Lexemes.Fun] = TokenType.Fun,
                [Lexemes.If] = TokenType.If,
                [Lexemes.Nil] = TokenType.Nil,
                [Lexemes.Or] = TokenType.Or,
                [Lexemes.Print] = TokenType.Print,
                [Lexemes.Return] = TokenType.Return,
                [Lexemes.Super] = TokenType.Super,
                [Lexemes.This] = TokenType.This,
                [Lexemes.True] = TokenType.True,
                [Lexemes.Var] = TokenType.Var,
                [Lexemes.While] = TokenType.While,
            };

        private bool IsAtEnd
            => _current >= _source.Length;

        public Lexer(
            string source)
            => _source = source;

        public IEnumerable<Token> ReadTokens()
        {
            while (!IsAtEnd)
            {
                _start = _current;
                ReadToken();
            }

            _tokens.Add(
                new Token(
                    TokenType.Eof,
                    string.Empty,
                    null,
                    _line));

            return _tokens
                .ToList()
                .AsReadOnly();
        }

        private void ReadToken()
        {
            var c = Advance();
            switch (c)
            {
                case Lexemes.LeftParen:
                    AddToken(TokenType.LeftParen);
                    break;
                case Lexemes.RightParen:
                    AddToken(TokenType.RightParen);
                    break;
                case Lexemes.LeftBrace:
                    AddToken(TokenType.LeftBrace);
                    break;
                case Lexemes.RightBrace:
                    AddToken(TokenType.RightBrace);
                    break;
                case Lexemes.Comma:
                    AddToken(TokenType.Comma);
                    break;
                case Lexemes.Dot:
                    AddToken(TokenType.Dot);
                    break;
                case Lexemes.Minus:
                    AddToken(TokenType.Minus);
                    break;
                case Lexemes.Plus:
                    AddToken(TokenType.Plus);
                    break;
                case Lexemes.Semicolon:
                    AddToken(TokenType.Semicolon);
                    break;
                case Lexemes.Star:
                    AddToken(TokenType.Star);
                    break;
                case Lexemes.Bang:
                    AddToken(
                        Match(Lexemes.Equal)
                            ? TokenType.BangEqual
                            : TokenType.Bang);
                    break;
                case Lexemes.Equal:
                    AddToken(
                        Match(Lexemes.Equal)
                            ? TokenType.EqualEqual
                            : TokenType.Equal);
                    break;
                case Lexemes.Less:
                    AddToken(
                        Match(Lexemes.Equal)
                            ? TokenType.LessEqual
                            : TokenType.Less);
                    break;
                case Lexemes.Greater:
                    AddToken(
                        Match(Lexemes.Equal)
                            ? TokenType.GreaterEqual
                            : TokenType.Greater);
                    break;
                case Lexemes.Slash:
                    if (Match(Lexemes.Slash))
                    {
                        // A comment goes until the end of the line.
                        while (Peek() != Lexemes.NewLine && !IsAtEnd) Advance();
                    }
                    else { AddToken(TokenType.Slash); }

                    break;

                case Lexemes.Whitespace:
                case Lexemes.CarriageReturn:
                case Lexemes.Tabulation:
                    // Ignore whitespace.
                    break;

                case Lexemes.NewLine:
                    _line++;
                    break;

                case Lexemes.StringDelimiter:
                    ReadString();
                    break;
                default:
                    if (IsDigit(c)) { ReadNumber(); }
                    else if (IsAlpha(c)) { ReadIdentifier(); }
                    else
                    {
                        Program.Error(
                            _line,
                            "Unexpected character.");
                    }

                    break;
            }
        }

        private void AddToken(
            TokenType tokenType,
            object? literal = null)
        {
            var text = _source.Substring(
                _start,
                _current - _start);
            _tokens.Add(
                new Token(
                    tokenType,
                    text,
                    literal,
                    _line));
        }

        private void ReadString()
        {
            while (Peek() != Lexemes.StringDelimiter && !IsAtEnd)
            {
                // Pulse supports multi-line strings.
                // That does mean we also need to update line when we hit a newline inside a string.
                if (Peek() == Lexemes.NewLine) { _line++; }

                Advance();
            }

            if (IsAtEnd)
            {
                Program.Error(
                    _line,
                    "Unterminated string.");
                return;
            }

            // The closing ".
            Advance();

            // Trim the surrounding quotes.
            var start = _start + 1;
            var len = _current - start - 1;
            var value = _source.Substring(
                start,
                len);
            AddToken(
                TokenType.String,
                value);
        }

        private void ReadNumber()
        {
            while (IsDigit(Peek())) { Advance(); }

            // Look for a fractional part.
            if (Peek() == Lexemes.Dot && IsDigit(PeekNext()))
            {
                // Consume the ".".
                Advance();
                while (IsDigit(Peek())) { Advance(); }
            }

            AddToken(
                TokenType.Number,
                double.Parse(
                    _source.Substring(
                        _start,
                        _current - _start),
                    CultureInfo.InvariantCulture));
        }

        private void ReadIdentifier()
        {
            while (IsAlphaNumeric(Peek())) { Advance(); }

            var text = _source.Substring(
                _start,
                _current - _start);

            if (_keywords.TryGetValue(
                text,
                out var tokenType))
            {
                AddToken(tokenType);
                return;
            }

            AddToken(TokenType.Identifier);
        }

        private char Advance()
        {
            _current++;
            return _source[_current - 1];
        }

        private bool Match(
            char expected)
        {
            if (IsAtEnd) { return false; }

            if (_source[_current] != expected) { return false; }

            // It’s like a conditional advance()
            // We only consume the current character if it’s what we’re
            // looking for.
            _current++;
            return true;
        }

        private char Peek()
            => IsAtEnd
                ? '\0'
                : _source[_current];

        private char PeekNext()
            => _current + 1 >= _source.Length
                ? '\0'
                : _source[_current + 1];

        private static bool IsDigit(
            char c)
            => c >= '0' && c <= '9';

        private static bool IsAlpha(
            char c)
            => (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c == '_';

        private static bool IsAlphaNumeric(
            char c)
            => IsAlpha(c) || IsDigit(c);
    }
}
