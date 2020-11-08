namespace Pulse.CodeAnalysis.Tests.FrontEnd.Errors
{
    using System.IO;
    using CodeAnalysis.FrontEnd;
    using CodeAnalysis.FrontEnd.Errors;
    using Moq;
    using Xunit;

    public class TextWriterErrorReporterTest
    {
        private readonly Mock<TextWriter> _textWriterMock =
            new Mock<TextWriter>();

        [Fact]
        public void ReportRuntimeError_Reports_RuntimeException()
        {
            // Arrange
            const string message = nameof(
                ReportRuntimeError_Reports_RuntimeException);
            const int line = 2;
            var reporter = new TextWriterErrorReporter(_textWriterMock.Object);

            // Act
            reporter.ReportRuntimeError(
                new RuntimeException(
                    new Token(
                        TokenType.And,
                        Lexemes.And,
                        null,
                        line),
                    message));

            // Assert
            Assert.False(reporter.HadSyntaxError);
            Assert.True(reporter.HadRuntimeError);

            _textWriterMock.Verify(
                x => x.WriteLine(
                    It.Is<string>(
                        s => s.Contains(message)
                            && s.Contains(line.ToString()))));
        }

        [Fact]
        public void ReportSyntaxError_Reports_ParseException()
        {
            // Arrange
            const string message = nameof(
                ReportSyntaxError_Reports_ParseException);
            const int line = 2;
            var reporter = new TextWriterErrorReporter(_textWriterMock.Object);

            // Act
            reporter.ReportSyntaxError(
                new ParseException(
                    new Token(
                        TokenType.And,
                        Lexemes.And,
                        null,
                        line),
                    message));

            // Assert
            Assert.True(reporter.HadSyntaxError);
            Assert.False(reporter.HadRuntimeError);

            _textWriterMock.Verify(
                x => x.WriteLine(
                    It.Is<string>(
                        s => s.Contains(message)
                            && s.Contains(line.ToString())
                            && s.Contains(Lexemes.And))));
        }

        [Fact]
        public void Reset_Resets_Error_Properties()
        {
            // Arrange
            const string message = nameof(
                Reset_Resets_Error_Properties);
            const int line = 2;
            var reporter = new TextWriterErrorReporter(_textWriterMock.Object);

            // Act
            reporter.ReportRuntimeError(
                new RuntimeException(
                    new Token(
                        TokenType.And,
                        Lexemes.And,
                        null,
                        line),
                    message));
            reporter.ReportSyntaxError(
                new ParseException(
                    new Token(
                        TokenType.And,
                        Lexemes.And,
                        null,
                        line),
                    message));
            reporter.Reset();

            // Assert
            Assert.False(reporter.HadRuntimeError);
            Assert.False(reporter.HadSyntaxError);
        }
    }
}
