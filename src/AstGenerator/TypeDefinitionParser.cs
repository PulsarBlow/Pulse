namespace Pulse.AstGenerator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class TypeDefinitionParser
    {
        public static IEnumerable<TypeDefinition> Parse(
            IEnumerable<string> sourceLines)
            => Parse(
                string.Join(
                    Environment.NewLine,
                    sourceLines));

        public static IEnumerable<TypeDefinition> Parse(
            string definitionSource)
        {
            if (string.IsNullOrWhiteSpace(definitionSource))
                throw new ArgumentNullException(nameof(definitionSource));

            var lines = definitionSource.Split(
                Environment.NewLine,
                StringSplitOptions.RemoveEmptyEntries);

            return lines.Select(ParseLine)
                .ToList();
        }

        private static TypeDefinition ParseLine(
            string line)
        {
            if (string.IsNullOrWhiteSpace(line))
                throw new ArgumentNullException(nameof(line));

            var parts = line.Split(
                ':',
                StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2
                || string.IsNullOrWhiteSpace(parts[0])
                || string.IsNullOrWhiteSpace(parts[1]))
                throw new FormatException("Line format is not valid");

            var typeName = parts[0]
                .Trim();
            var fieldDefinitions = ParseFieldList(parts[1]);

            return new TypeDefinition(
                typeName,
                fieldDefinitions);
        }

        private static IEnumerable<MemberDefinition> ParseFieldList(
            string fieldList)
        {
            if (string.IsNullOrWhiteSpace(fieldList))
                throw new ArgumentNullException(nameof(fieldList));

            var parts = fieldList
                .Trim()
                .Split(
                    ',',
                    StringSplitOptions.RemoveEmptyEntries);

            return parts
                .Select(ParseFieldDefinition)
                .ToList();
        }

        private static MemberDefinition ParseFieldDefinition(
            string fieldDefinition)
        {
            if (string.IsNullOrWhiteSpace(fieldDefinition))
                throw new ArgumentNullException(nameof(fieldDefinition));

            var parts = fieldDefinition
                .Trim()
                .Split(' ');
            if (parts.Length != 2)
                throw new FormatException(
                    "Field definition format is not valid");

            var typeName = parts[0]
                .Trim();
            var identifierName = parts[1]
                .Trim();

            return new MemberDefinition(
                typeName,
                identifierName);
        }
    }
}
