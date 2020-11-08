namespace Pulse.CodeAnalysis.FrontEnd.Errors
{
    using System.IO;

    public class TextWriterErrorReporter
        : IErrorReporter
    {
        private readonly TextWriter _textWriter;

        public bool HadSyntaxError { get; private set; }
        public bool HadRuntimeError { get; private set; }

        public TextWriterErrorReporter(
            TextWriter textWriter)
            => _textWriter = textWriter;

        public void Reset()
        {
            HadSyntaxError = false;
            HadRuntimeError = false;
        }

        public void ReportSyntaxError(
            int line,
            string message)
            => Report(
                line,
                string.Empty,
                message);

        public void ReportSyntaxError(
            ParseException error)
            => Report(
                error.Token,
                error.Message);

        public void ReportRuntimeError(
            RuntimeException error)
        {
            HadRuntimeError = true;
            _textWriter.WriteLine(
                $"Runtime error: {error.Message}\n[line: {error.Token.Line}]");
        }

        private void Report(
            Token token,
            string message)
            => Report(
                token.Line,
                token.Type == TokenType.Eof
                    ? " at end"
                    : $" at '{token.Lexeme}'",
                message);

        private void Report(
            int line,
            string where,
            string message)
        {
            HadSyntaxError = true;
            _textWriter.WriteLine($"[line {line}] Error{where}: {message}");
        }
    }
}
