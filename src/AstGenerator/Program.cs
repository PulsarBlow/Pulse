namespace Pulse.AstGenerator
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    public static class Program
    {
        private const int ExUsage = 64;

        public static async Task Main(
            string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: pulse_ast <output directory>");
                Environment.Exit(ExUsage);
            }

            var outputDir = args[0];

            // Generate Expression AST
            await GenerateAstAsync(
                    new AstGenerationOptions
                    {
                        AstKind = AstKind.Expressions,
                        TypeDescriptions = new[]
                        {
                            "BinaryExpression   : Expression left, Token operator, Expression right",
                            "GroupingExpression : Expression expression",
                            "LiteralExpression  : object value",
                            "UnaryExpression    : Token operator, Expression right",
                        },
                        BaseNamespace = "Pulse.CodeAnalysis.FrontEnd",
                        BaseTypeName = "Expression",
                        FileName = "Expression.cs",
                        OutputDir = outputDir,
                    })
                .ConfigureAwait(false);

            // Generate Statement AST
            await GenerateAstAsync(
                    new AstGenerationOptions
                    {
                        AstKind = AstKind.Statements,
                        TypeDescriptions = new[]
                        {
                            "ExpressionStatement : Expression expression",
                            "PrintStatement      : Expression expression",
                        },
                        BaseNamespace = "Pulse.CodeAnalysis.FrontEnd",
                        BaseTypeName = "Statement",
                        FileName = "Statement.cs",
                        OutputDir = outputDir,
                    })
                .ConfigureAwait(false);
        }

        private static async Task GenerateAstAsync(
            AstGenerationOptions options)
        {
            var typeDefinitions = TypeDefinitionParser.Parse(
                options.TypeDescriptions);

            var builder = new AstBuilder(
                options.BaseNamespace,
                options.BaseTypeName);

            var outputFile = Path.Combine(
                options.OutputDir,
                options.FileName);
            var writer = new AstWriter(
                builder,
                outputFile);

            await writer.WriteAstAsync(
                options.AstKind,
                typeDefinitions);
        }
    }
}
