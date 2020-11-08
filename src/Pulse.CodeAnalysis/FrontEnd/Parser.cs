namespace Pulse.CodeAnalysis.FrontEnd
{
    using System.Collections.Generic;
    using System.Linq;
    using Errors;

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
    public class Parser
    {
        private readonly IErrorReporter _errorReporter;
        private readonly List<Token> _tokens = new List<Token>();
        private int _current;

        public Parser(
            IErrorReporter errorReporter,
            IEnumerable<Token> tokens)
        {
            _errorReporter = errorReporter;
            _tokens.AddRange(tokens);
        }

        /// <summary>
        /// Parse the <see cref="Token"/> collection used
        /// to instantiates the Parser into an AST
        /// </summary>
        /// <returns>The parsed Expression (AST)</returns>
        /// <exception cref="ParseException">If the parsing fails</exception>
        public Expression? Parse()
        {
            try { return Expression(); }
            catch (ParseException ex)
            {
                // We don't want the parser to crash on error
                _errorReporter.ReportSyntaxError(ex);
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
                expression = new BinaryExpression(
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
                expression = new BinaryExpression(
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
                expression = new BinaryExpression(
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
                expression = new BinaryExpression(
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
            return new UnaryExpression(
                op,
                right);
        }

        private Expression Primary()
        {
            if (Match(TokenType.False)) { return new LiteralExpression(false); }

            if (Match(TokenType.True)) { return new LiteralExpression(true); }

            if (Match(TokenType.Nil)) { return new LiteralExpression(null); }

            if (Match(
                TokenType.Number,
                TokenType.String))
            {
                return new LiteralExpression(
                    Previous()
                        .Literal);
            }

            if (Match(TokenType.LeftParen))
            {
                Expression expression = Expression();
                Consume(
                    TokenType.RightParen,
                    "Expect ')' after expression.");
                return new GroupingExpression(expression);
            }

            throw new ParseException(
                Peek(),
                "Expect expression.");
        }

        private void Consume(
            TokenType tokenType,
            string message)
        {
            if (!Check(tokenType))
            {
                throw new ParseException(
                    Peek(),
                    message);
            }

            Advance();
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
            if (IsAtEnd()) { return false; }

            return Peek()
                    .Type
                == type;
        }

        private void Advance()
        {
            if (!IsAtEnd()) { _current++; }
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
