namespace Pulse.CodeAnalysis.Tests
{
    using CodeAnalysis.FrontEnd;
    using CodeAnalysis.FrontEnd.Errors;
    using Moq;

    public partial class InterpreterTest
    {
        private readonly Mock<IErrorReporter> _errorReporterMock =
            new Mock<IErrorReporter>();

        private Interpreter CreateInterpreter()
            => new Interpreter(_errorReporterMock.Object);

        private static Token CreateOperator(
            TokenType tokenType,
            char lexeme)
            => CreateOperator(
                tokenType,
                lexeme.ToString());

        private static Token CreateOperator(
            TokenType tokenType,
            string lexeme)
            => new Token(
                tokenType,
                lexeme,
                null,
                1);
    }
}
