namespace Pulse.Interpreter.FrontEnd
{
    public class Token
    {
        public TokenType Type { get; }

        public string Lexeme { get; }

        public object? Literal { get; }

        public int Line { get; }

        public Token(
            TokenType tokenType,
            string lexeme,
            object? literal,
            int line)
        {
            Type = tokenType;
            Lexeme = lexeme;
            Literal = literal;
            Line = line;
        }

        public override string ToString()
            => $"TokenType: {Type}, Lexeme: {Lexeme}, Literal: {Literal ?? "-"}, Line: {Line}";
    }
}
