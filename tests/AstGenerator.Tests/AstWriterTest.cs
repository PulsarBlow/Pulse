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
            var fileName = Path.GetRandomFileName();
            var builder = new AstBuilder(
                baseNamespace,
                "SomeBaseType");
            var writer = new AstWriter(
                builder,
                fileName);

            await writer.WriteAstAsync(CreateTypeDefinitions());

            var content = await File.ReadAllTextAsync(fileName);
            Assert.Contains(
                baseNamespace,
                content);
        }

        private static IEnumerable<TypeDefinition> CreateTypeDefinitions()
        {
            return new[]
            {
                new TypeDefinition(
                    "SomeType",
                    new[]
                    {
                        new MemberDefinition(
                            "SomeMemberType",
                            "operator"),
                    }),
            };
        }
    }
}
