namespace Pulse.CodeAnalysis.FrontEnd.Errors
{
    public interface IErrorReporter
    {
        bool HadSyntaxError { get; }
        bool HadRuntimeError { get; }

        void ReportSyntaxError(
            int line,
            string message);

        void ReportSyntaxError(
            ParseException error);

        void ReportRuntimeError(
            RuntimeException error);

        void Reset();
    }
}
