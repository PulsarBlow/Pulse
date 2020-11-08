namespace Pulse
{
    using System;
    using System.IO;
    using System.Text;
    using CodeAnalysis;
    using CodeAnalysis.FrontEnd;
    using CodeAnalysis.FrontEnd.Errors;

    public static class Program
    {
        public static void Main(
            string[] args)
        {
            if (args.Length > 1)
            {
                Console.WriteLine("Usage: pulse [script]");
                Environment.Exit(ExitCode.ExUsage);
            }
            else if (args.Length == 1) { RunFile(args[0]); }
            else { RunPrompt(); }
        }

        private static void RunFile(
            string path)
        {
            var source = File.ReadAllText(
                Path.GetFullPath(path),
                Encoding.UTF8);

            IErrorReporter errorReporter =
                new TextWriterErrorReporter(Console.Out);

            Run(
                source,
                errorReporter);

            if (errorReporter.HadSyntaxError) { Environment.Exit(ExitCode.ExDataErr); }

            if (errorReporter.HadRuntimeError) { Environment.Exit(ExitCode.ExSoftware); }
        }

        private static void RunPrompt()
        {
            IErrorReporter errorReporter =
                new TextWriterErrorReporter(Console.Out);
            while (true)
            {
                Console.Write("> ");
                var source = Console.ReadLine();
                if (source == null) { break; }

                Run(
                    source,
                    errorReporter);
                errorReporter.Reset();
            }
        }

        private static void Run(
            string source,
            IErrorReporter errorReporter)
        {
            var lexer = new Lexer(
                source,
                errorReporter);
            var tokens = lexer.ReadTokens();
            var parser = new Parser(
                errorReporter,
                tokens);
            var expression = parser.Parse();

            if (errorReporter.HadSyntaxError || expression == null) { return; }

            new Interpreter(errorReporter).Interpret(expression);
        }
    }
}
