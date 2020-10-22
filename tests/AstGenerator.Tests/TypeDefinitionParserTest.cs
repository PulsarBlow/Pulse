namespace Pulse.AstGenerator.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AstGenerator;
    using Xunit;

    public class TypeDefinitionParserTest
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Parse_Throws_When_Source_Is_Not_Valid(
            string source)
        {
            Assert.Throws<ArgumentNullException>(
                () => TypeDefinitionParser.Parse(source));
        }

        [Fact]
        public void Parse_Throws_When_TypeDefinition_Format_Is_Not_Valid()
        {
            const string source = "e";
            Assert.Throws<FormatException>(
                () => TypeDefinitionParser.Parse(source));
        }

        [Fact]
        public void Parse_Throws_When_Line_Format_Is_Not_Valid()
        {
            const string source = "Binary:";
            var ex = Assert.Throws<FormatException>(
                () => TypeDefinitionParser.Parse(source));
            Assert.Contains(
                ex.Message,
                "Line format is not valid");
        }

        [Theory]
        [MemberData(nameof(Parse_Expected_Test_Cases))]
        internal void Parse_Returns_Expected_Definitions(
            string source,
            IEnumerable<TypeDefinition> expected)
        {
            var result = TypeDefinitionParser.Parse(source);

            var typeDefinitions = expected.ToList();
            var inspectors = typeDefinitions.Select(TypeDefinitionInspector)
                .ToArray();

            Assert.Collection(
                result,
                inspectors);
        }

        public static IEnumerable<object[]> Parse_Expected_Test_Cases()
        {
            yield return new object[]
            {
                "Binary    : Expression left, Token operator, Expression right",
                new[]
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
                },
            };

            yield return new object[]
            {
                "Grouping    : Expression expression\nLiteral    : object value\nUnary    : Token operator, Expression right",
                new[]
                {
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
                },
            };
        }

        private static Action<TypeDefinition> TypeDefinitionInspector(
            TypeDefinition expected)
        {
            return x =>
            {
                Assert.Equal(
                    expected.TypeName,
                    x.TypeName);

                Assert.Collection(
                    x.Members,
                    expected.Members.Select(FieldDefinitionInspector)
                        .ToArray());
            };
        }

        private static Action<MemberDefinition> FieldDefinitionInspector(
            MemberDefinition expected)
        {
            return x =>
            {
                Assert.Equal(
                    expected.TypeName,
                    x.TypeName);
                Assert.Equal(
                    expected.IdentifierName,
                    x.IdentifierName);
            };
        }
    }
}
