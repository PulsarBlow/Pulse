namespace Pulse.AstGenerator
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;

    internal class AstWriter
    {
        private readonly AstBuilder _builder;
        private readonly string _outputPath;

        public AstWriter(
            AstBuilder builder,
            string outputPath)
        {
            _builder = builder
                ?? throw new ArgumentNullException(nameof(builder));

            if (string.IsNullOrWhiteSpace(outputPath))
            {
                throw new ArgumentNullException(nameof(outputPath));
            }

            _outputPath = outputPath;
        }

        public async Task WriteAstAsync(
            IEnumerable<TypeDefinition> typeDefinitions)
        {
            var ast = _builder.BuildAst(typeDefinitions);
            await using var writer = new StreamWriter(
                _outputPath,
                false,
                Encoding.UTF8);
            await writer.WriteAsync(ast);
        }
    }
}
