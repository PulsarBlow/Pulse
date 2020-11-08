namespace Pulse.AstGenerator.Tests
{
    using System.Collections.Generic;
    using Snapper;
    using Xunit;

    public class AstBuilderTest
    {
        private const int IndentSize = 2;

        [Fact]
        public void BuildAst_Returns_Expected_Ast()
        {
            var typeDefinitions = CreateTypeDefinitions();
            // We do not add file header because
            // it is not deterministic.
            var builder = new AstBuilder(
                "AstGenerator.Tests.Ast",
                IndentSize);

            builder.BuildAst(typeDefinitions)
                .ShouldMatchSnapshot();
        }

        private static IEnumerable<TypeDefinition> CreateTypeDefinitions()
        {
            return new[]
            {
                new TypeDefinition(
                    "Binary",
                    new[]
                    {
                        new MemberDefinition(
                            "Expression",
                            "left"),
                        new MemberDefinition(
                            "Token",
                            "operator"),
                        new MemberDefinition(
                            "Expression",
                            "right"),
                    }),
                new TypeDefinition(
                    "Grouping",
                    new[]
                    {
                        new MemberDefinition(
                            "Expression",
                            "expression"),
                    }),
                new TypeDefinition(
                    "Literal",
                    new[]
                    {
                        new MemberDefinition(
                            "object",
                            "value"),
                    }),
                new TypeDefinition(
                    "Unary",
                    new[]
                    {
                        new MemberDefinition(
                            "Token",
                            "operator"),
                        new MemberDefinition(
                            "Expression",
                            "right"),
                    }),
            };
        }
    }
}
