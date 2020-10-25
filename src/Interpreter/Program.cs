﻿namespace Pulse.Interpreter
{
    using System;
    using System.IO;
    using System.Text;
    using FrontEnd;

    public static class Program
    {
        private static bool _hadError;

        public static void Main(
            string[] args)
        {
            if (args.Length > 1)
            {
                Console.WriteLine("Usage: pulse [script]");
                Environment.Exit(64);
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
            Run(source);

            if (_hadError) Environment.Exit(65);
        }

        private static void RunPrompt()
        {
            while (true)
            {
                Console.Write("> ");
                var source = Console.ReadLine();
                if (source == null) { break; }

                Run(source);
                _hadError = false;
            }
        }

        private static void Run(
            string source)
        {
            var lexer = new Lexer(source);
            var tokens = lexer.ReadTokens();
            var parser = new Parser(tokens);
            var expression = parser.Parse();

            if (_hadError || expression == null) return;
            Console.WriteLine(new AstPrinter().Print(expression));
        }

        internal static void Error(
            int line,
            string message)
        {
            Report(
                line,
                string.Empty,
                message);
        }

        internal static void Error(
            Token token,
            string message)
        {
            Report(
                token.Line,
                token.Type == TokenType.Eof
                    ? " at end"
                    : $" at '{token.Lexeme}'",
                message);
        }

        private static void Report(
            int line,
            string where,
            string message)
        {
            Console.WriteLine($"[line {line}] Error{where}: {message}");
        }
    }
}
