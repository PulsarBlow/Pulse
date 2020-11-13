namespace Pulse.AstGenerator.Tests
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Xunit;

    public class AstWriterTest
    {
        [Fact]
        public async Task WriteAstAsync()
        {
            const string baseNamespace = "Pulse.AstGenerator.Tests";
            const string baseTypeName = "ExpressionTest";
            var fileName = Path.GetRandomFileName();
            var builder = new AstBuilder(
                baseNamespace,
                baseTypeName);
            var writer = new AstWriter(
                builder,
                fileName);

            await writer.WriteAstAsync(
                AstKind.Expressions,
                CreateTypeDefinitions());

            var content = await File.ReadAllTextAsync(fileName);
            Assert.Contains(
                baseNamespace,
                content);
        }

        private static IEnumerable<TypeDescriptor> CreateTypeDefinitions()
        {
            return new[]
            {
                new TypeDescriptor(
                    "SomeType",
                    new[]
                    {
                        new MemberDescriptor(
                            "SomeMemberType",
                            "operator"),
                    }),
            };
        }
    }
}
