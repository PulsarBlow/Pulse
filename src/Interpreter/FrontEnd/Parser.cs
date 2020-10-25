namespace Pulse.Interpreter.FrontEnd
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Recursive descent parser
    /// </summary>
    // Parse for the following grammar:
    // expression     → equality ;
    // equality       → comparison ( ( "!=" | "==" ) comparison )*;
    // comparison     → term ( ( ">" | ">=" | "<" | "<=" ) term )*;
    // term           → factor ( ( "-" | "+" ) factor )*;
    // factor         → unary ( ( "/" | "*" ) unary )*;
    // unary          → ( "!" | "-" ) unary | primary;
    // primary        → NUMBER | STRING | "true" | "false" | "nil"
    //                | "(" expression ")";
    internal class Parser
    {
        private readonly List<Token> _tokens = new List<Token>();
        private int _current;

        public Parser(
            IEnumerable<Token> tokens)
        {
            _tokens.AddRange(tokens);
        }

        public Expression? Parse()
        {
            try { return Expression(); }
            catch (ParseException)
            {
                // That's OK.
                // We don't want the parser to crash on invalid syntax.
                // It will handle error in panic mode.
                // https://compilers.iecc.com/comparch/article/03-04-035
                return null;
            }
        }

        private Expression Expression()
            => Equality();

        private Expression Equality()
        {
            Expression expression = Comparison();

            while (Match(
                TokenType.BangEqual,
                TokenType.EqualEqual))
            {
                Token op = Previous();
                Expression right = Comparison();
                expression = new Expression.Binary(
                    expression,
                    op,
                    right);
            }

            return expression;
        }

        private Expression Comparison()
        {
            Expression expression = Term();

            while (Match(
                TokenType.Greater,
                TokenType.GreaterEqual,
                TokenType.Less,
                TokenType.LessEqual))
            {
                Token op = Previous();
                Expression right = Term();
                expression = new Expression.Binary(
                    expression,
                    op,
                    right);
            }

            return expression;
        }

        private Expression Term()
        {
            Expression expression = Factor();

            while (Match(
                TokenType.Minus,
                TokenType.Plus))
            {
                Token op = Previous();
                Expression right = Factor();
                expression = new Expression.Binary(
                    expression,
                    op,
                    right);
            }

            return expression;
        }

        private Expression Factor()
        {
            Expression expression = Unary();

            while (Match(
                TokenType.Slash,
                TokenType.Star))
            {
                Token op = Previous();
                Expression right = Unary();
                expression = new Expression.Binary(
                    expression,
                    op,
                    right);
            }

            return expression;
        }

        private Expression Unary()
        {
            if (!Match(
                TokenType.Bang,
                TokenType.Minus)) { return Primary(); }

            Token op = Previous();
            Expression right = Unary();
            return new Expression.Unary(
                op,
                right);
        }

        private Expression Primary()
        {
            if (Match(TokenType.False)) return new Expression.Literal(false);
            if (Match(TokenType.True)) return new Expression.Literal(true);
            if (Match(TokenType.Nil)) return new Expression.Literal(null);

            if (Match(
                TokenType.Number,
                TokenType.String))
            {
                return new Expression.Literal(
                    Previous()
                        .Literal);
            }

            if (Match(TokenType.LeftParen))
            {
                Expression expression = Expression();
                Consume(
                    TokenType.RightParen,
                    "Expect ')' after expression.");
                return new Expression.Grouping(expression);
            }

            throw Error(
                Peek(),
                "Expect expression.");
        }

        private void Consume(
            TokenType tokenType,
            string message)
        {
            if (!Check(tokenType))
            {
                throw Error(
                    Peek(),
                    message);
            }

            Advance();
        }

        private static ParseException Error(
            Token token,
            string message)
        {
            Program.Error(
                token,
                message);
            return new ParseException(message);
        }

        private bool Match(
            params TokenType[] tokenTypes)
        {
            if (!tokenTypes.Any(Check)) { return false; }

            Advance();
            return true;
        }

        private bool Check(
            TokenType type)
        {
            if (IsAtEnd()) return false;

            return Peek()
                    .Type
                == type;
        }

        private void Advance()
        {
            if (!IsAtEnd()) _current++;
        }

        private bool IsAtEnd()
            => Peek()
                    .Type
                == TokenType.Eof;

        private Token Peek()
            => _tokens[_current];

        private Token Previous()
            => _tokens[_current - 1];
    }
}
