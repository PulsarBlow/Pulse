namespace Pulse.AstGenerator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    // A type definition is a string source which defines how types
    // should be parsed and converted into (Type|Member)Descriptor.
    // The definition grammar is the following
    // Definition       → TypeDefinition* EOF
    // TypeDefinition   → TypeName : MemberDefinition ("," MemberDefinition)* EOL
    // MemberDefinition → MemberType : MemberName
    // TypeName         → NUMBER | STRING
    // MemberType       → NUMBER | STRING
    // MemberName       → NUMBER | STRING
    internal static class TypeDefinitionParser
    {
        public static IEnumerable<TypeDescriptor> Parse(
            IEnumerable<string> sourceLines)
            => Parse(
                string.Join(
                    Environment.NewLine,
                    sourceLines));

        public static IEnumerable<TypeDescriptor> Parse(
            string definitionSource)
        {
            if (string.IsNullOrWhiteSpace(definitionSource))
            {
                throw new ArgumentNullException(nameof(definitionSource));
            }

            var lines = definitionSource.Split(
                Environment.NewLine,
                StringSplitOptions.RemoveEmptyEntries);

            return lines.Select(ParseType)
                .ToList();
        }

        private static TypeDescriptor ParseType(
            string line)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                throw new ArgumentNullException(nameof(line));
            }

            var parts = line.Split(
                ':',
                StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2
                || string.IsNullOrWhiteSpace(parts[0])
                || string.IsNullOrWhiteSpace(parts[1]))
            {
                throw new FormatException(
                    $"Type definition format is not valid [line={line}]");
            }

            var typeName = parts[0]
                .Trim();
            var members = ParseMembers(parts[1]);
            return new TypeDescriptor(
                typeName,
                members);
        }

        private static IEnumerable<MemberDescriptor> ParseMembers(
            string def)
        {
            if (string.IsNullOrWhiteSpace(def))
            {
                throw new ArgumentNullException(nameof(def));
            }

            var parts = def
                .Trim()
                .Split(
                    ',',
                    StringSplitOptions.RemoveEmptyEntries);

            return parts
                .Select(ParseMember)
                .ToList();
        }

        private static MemberDescriptor ParseMember(
            string def)
        {
            if (string.IsNullOrWhiteSpace(def))
            {
                throw new ArgumentNullException(nameof(def));
            }

            var parts = def
                .Trim()
                .Split(' ');
            if (parts.Length != 2)
            {
                throw new FormatException(
                    $"Member definition format is not valid [def={def}]");
            }

            var typeName = parts[0]
                .Trim();
            var identifierName = parts[1]
                .Trim();

            return new MemberDescriptor(
                typeName,
                identifierName);
        }
    }
}
