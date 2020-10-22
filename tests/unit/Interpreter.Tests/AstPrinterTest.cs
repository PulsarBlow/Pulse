namespace Pulse.Interpreter.Tests
{
    using Xunit;
    using Interpreter.FrontEnd;

    public class AstPrinterTest
    {
        [Fact]
        public void Print_Returns_Expected_Result()
        {
            Expression expression = new Expression.Binary(
                new Expression.Unary(
                    new Token(
                        TokenType.Minus,
                        Lexemes.Minus.ToString(),
                        null,
                        1),
                    new Expression.Literal(123)),
                new Token(
                    TokenType.Star,
                    Lexemes.Star.ToString(),
                    null,
                    1),
                new Expression.Grouping(new Expression.Literal(45.67)));

            var printer = new AstPrinter();
            var result = printer.Print(expression);

            Assert.Equal(
                "(* (- 123) (group 45.67))",
                result);
        }
    }
}
