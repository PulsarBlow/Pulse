namespace Pulse.CodeAnalysis.Tests
{
    using System;
    using CodeAnalysis.FrontEnd;
    using Xunit;

    internal static class ExpressionAssertions
    {
        public static Action<Expression> NumberInspector(
            double value)
            => expression => Literal(
                value,
                expression);

        public static Action<Expression> StringInspector(
            string value)
            => expression => Literal(
                value,
                expression);

        public static Action<Expression> BooleanInspector(
            bool value)
            => expression => Literal(
                value,
                expression);

        public static void Literal(
            double value,
            Expression expression)
        {
            var literal = Assert.IsType<LiteralExpression>(expression);
            Assert.Equal(
                value,
                literal.Value);
        }

        public static void Literal(
            string value,
            Expression expression)
        {
            var literal = Assert.IsType<LiteralExpression>(expression);
            Assert.Equal(
                value,
                literal.Value);
        }

        public static void Literal(
            bool value,
            Expression expression)
        {
            var literal = Assert.IsType<LiteralExpression>(expression);
            Assert.Equal(
                value,
                literal.Value);
        }
    }
}
