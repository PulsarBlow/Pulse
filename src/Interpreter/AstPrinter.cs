namespace Pulse.Interpreter
{
    using System;
    using System.Globalization;
    using System.Text;
    using FrontEnd;

    internal class AstPrinter : IVisitor<string>
    {
        public string Print(
            Expression expression)
            => expression.Accept(this);

        public string VisitBinaryExpression(
            BinaryExpression expression)
            => Parenthesize(
                expression.Operator.Lexeme,
                expression.Left,
                expression.Right);

        public string VisitGroupingExpression(
            GroupingExpression expression)
            => Parenthesize(
                "group",
                expression.Expression);

        public string VisitLiteralExpression(
            LiteralExpression expression)
            => Convert.ToString(
                    expression.Value,
                    CultureInfo.InvariantCulture)
                ?? string.Empty;

        public string VisitUnaryExpression(
            UnaryExpression expression)
            => Parenthesize(
                expression.Operator.Lexeme,
                expression.Right);

        private string Parenthesize(
            string name,
            params Expression[] expressions)
        {
            var builder = new StringBuilder();
            builder
                .Append("(")
                .Append(name);
            foreach (var expression in expressions)
            {
                builder.Append(" ");
                builder.Append(expression.Accept(this));
            }

            builder.Append(")");
            return builder.ToString();
        }
    }
}
