namespace Pulse.AstGenerator.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
                "Type definition format is not valid [line=Binary:]",
                ex.Message);
        }

        [Theory]
        [MemberData(nameof(Parse_Expected_Test_Cases))]
        internal void Parse_Returns_Expected_Definitions(
            string source,
            IEnumerable<TypeDescriptor> expected)
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
                },
            };

            yield return new object[]
            {
                "Grouping    : Expression expression\nLiteral    : object value\nUnary    : Token operator, Expression right",
                new[]
                {
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
                },
            };
        }

        private static Action<TypeDescriptor> TypeDefinitionInspector(
            TypeDescriptor expected)
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

        private static Action<MemberDescriptor> FieldDefinitionInspector(
            MemberDescriptor expected)
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
