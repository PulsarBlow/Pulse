namespace Pulse.Interpreter
{
    using System.Text;
    using FrontEnd;

    internal class AstPrinter : Expression.IVisitor<string>
    {
        public string Print(
            Expression expression)
            => expression.Accept(this);

        public string VisitBinaryExpression(
            Expression.Binary expression)
            => Parenthesize(
                expression.Operator.Lexeme,
                expression.Left,
                expression.Right);

        public string VisitGroupingExpression(
            Expression.Grouping expression)
            => Parenthesize(
                "group",
                expression.Expression);

        public string VisitLiteralExpression(
            Expression.Literal expression)
            => expression.Value.ToString() ?? string.Empty;

        public string VisitUnaryExpression(
            Expression.Unary expression)
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
