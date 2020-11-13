namespace Pulse.CodeAnalysis.Tests
{
    using CodeAnalysis.FrontEnd;
    using CodeAnalysis.FrontEnd.Errors;
    using Moq;
    using Xunit;

    public partial class InterpreterTest
    {
        private readonly Mock<IErrorReporter> _errorReporterMock =
            new Mock<IErrorReporter>();

        [Fact]
        public void Interpret_Statement_Executes_Successfully()
        {
            // Arrange
            var statementMock = new Mock<Statement>();
            var interpreter = CreateInterpreter();

            // Act
            interpreter.Interpret(new[] { statementMock.Object });

            // Assert
            statementMock.Verify(
                x => x.Accept(interpreter),
                Times.Once);
        }

        [Fact]
        public void Interpret_Reports_Runtime_Error()
        {
            // Arrange
            const string errorMessage = "ERROR";
            var statementMock = new Mock<Statement>();
            statementMock.Setup(x => x.Accept(It.IsAny<IStatementVisitor>()))
                .Throws(
                    new RuntimeException(
                        CreateOperator(
                            TokenType.Plus,
                            Lexemes.Plus),
                        errorMessage));
            var interpreter = CreateInterpreter();

            // Act
            interpreter.Interpret(new[] { statementMock.Object });

            // Assert
            _errorReporterMock.Verify(
                x => x.ReportRuntimeError(
                    It.Is<RuntimeException>(e => e.Message == errorMessage)));
        }

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
