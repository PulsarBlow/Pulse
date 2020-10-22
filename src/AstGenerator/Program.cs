namespace Pulse.AstGenerator
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    public static class Program
    {
        public static async Task Main(
            string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: generate_ast <output directory>");
                Environment.Exit(64);
            }

            var outputDir = args[0];

            var typeDefinitions = TypeDefinitionParser.Parse(
                new[]
                {
                    "Binary   : Expression left, Token operator, Expression right",
                    "Grouping : Expression expression",
                    "Literal  : object value",
                    "Unary    : Token operator, Expression right",
                });

            const int indentSize= 4;
            const string baseName = "Expression";

            var builder = new AstBuilder(
                "Pulse.Interpreter.FrontEnd",
                "Expression",
                indentSize);

            var outputFile = Path.Combine(
                outputDir,
                $"{baseName}.cs");
            var writer = new AstWriter(
                builder,
                outputFile);

            await writer.WriteAstAsync(typeDefinitions);
        }
    }
}
