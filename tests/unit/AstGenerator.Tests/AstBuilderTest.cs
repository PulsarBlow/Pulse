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
                "ExpressionTest",
                IndentSize);

            builder.BuildAst(
                    AstKind.Expressions,
                    typeDefinitions)
                .ShouldMatchSnapshot();
        }

        private static IEnumerable<TypeDescriptor> CreateTypeDefinitions()
        {
            return new[]
            {
                new TypeDescriptor(
                    "Binary",
                    new[]
                    {
                        new MemberDescriptor(
                            "Expression",
                            "left"),
                        new MemberDescriptor(
                            "Token",
                            "operator"),
                        new MemberDescriptor(
                            "Expression",
                            "right"),
                    }),
                new TypeDescriptor(
                    "Grouping",
                    new[]
                    {
                        new MemberDescriptor(
                            "Expression",
                            "expression"),
                    }),
                new TypeDescriptor(
                    "Literal",
                    new[]
                    {
                        new MemberDescriptor(
                            "object",
                            "value"),
                    }),
                new TypeDescriptor(
                    "Unary",
                    new[]
                    {
                        new MemberDescriptor(
                            "Token",
                            "operator"),
                        new MemberDescriptor(
                            "Expression",
                            "right"),
                    }),
            };
        }
    }
}
