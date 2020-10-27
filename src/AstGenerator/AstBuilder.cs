namespace Pulse.AstGenerator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    internal class AstBuilder
    {
        private const int DefaultIdentSize = 4;
        private readonly StringBuilder _builder = new StringBuilder();
        private int _indentLevel;
        private readonly string _baseNamespace;
        private readonly int _indentSize;

        public AstBuilder(
            string baseNamespace,
            int indentSize = DefaultIdentSize)
        {
            if (string.IsNullOrWhiteSpace(baseNamespace))
                throw new ArgumentNullException(nameof(baseNamespace));

            _baseNamespace = baseNamespace;

            _indentSize = indentSize > 0
                ? indentSize
                : DefaultIdentSize;
        }

        public string BuildAst(
            IEnumerable<TypeDefinition> typeDefinitions)
        {
            _builder.Clear();
            WriteAst(typeDefinitions);
            return _builder.ToString();
        }

        private void WriteAst(
            IEnumerable<TypeDefinition> typeDefinitions)
        {
            if (typeDefinitions == null)
                throw new ArgumentNullException(nameof(typeDefinitions));

            // Avoid multiple enumeration
            var definitions = typeDefinitions.ToList();
            if (definitions.Count == 0) return;

            WriteFileHeader();
            WriteLine($"namespace {_baseNamespace}");
            OpenBlock();

            WriteVisitor(definitions);
            WriteBaseType();
            foreach (var type in definitions) { WriteType(type); }

            CloseBlock();
        }

        private void WriteFileHeader()
        {
            var asm = Assembly.GetAssembly(typeof(AstBuilder));
            if (asm == null) return;

            var generatorName = asm
                .GetName()
                .Name;
            var generatorVersion = asm
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                ?.InformationalVersion;

            WriteLine(
                "//------------------------------------------------------------------------------");
            WriteLine("// <auto-generated>");
            WriteLine("//     This code was generated by a tool.");
            WriteLine($"//     Generator name    : {generatorName}");
            WriteLine($"//     Generator version : {generatorVersion}");
            WriteLine("//");
            WriteLine(
                "//     Changes to this file may cause incorrect behavior and will be lost if");
            WriteLine("//     the code is regenerated.");
            WriteLine("// </auto-generated>");
            WriteLine(
                "//------------------------------------------------------------------------------");
        }

        private void WriteVisitor(
            IEnumerable<TypeDefinition> types)
        {
            WriteLine("internal interface IVisitor<out T>");
            OpenBlock();
            foreach (var definition in types)
            {
                WriteLine(
                    $"T Visit{definition.TypeName}Expression({definition.TypeName}Expression expression);");
            }

            CloseBlock();
        }

        private void WriteBaseType()
        {
            WriteLine($"internal abstract class Expression");
            OpenBlock();

            WriteLine("public abstract T Accept<T>(IVisitor<T> visitor);");
            CloseBlock();
        }

        private void WriteType(
            TypeDefinition type)
        {
            WriteLine($"internal sealed class {type.TypeName}Expression : Expression");
            OpenBlock();
            WriteProperties(type);
            WriteLine();
            WriteConstructor(type);
            WriteLine();
            WriteLine("public override T Accept<T>(IVisitor<T> visitor)");
            Indent();
            WriteLine($"=> visitor.Visit{type.TypeName}Expression(this);");
            Unindent();
            CloseBlock();
        }

        private void WriteProperties(
            TypeDefinition type)
        {
            foreach (var member in type.Members)
            {
                WriteLine(
                    $"public {member.TypeName} {FormatPropertyIdentifier(member.IdentifierName)} {{ get; }}");
            }
        }

        private void WriteConstructor(
            TypeDefinition type)
        {
            var parameters = string.Join(
                ",",
                type.Members.Select(
                    x => $"{x.TypeName} {FormatParameterName(x.IdentifierName)}"));
            WriteLine($"public {type.TypeName}Expression({parameters})");

            OpenBlock();
            foreach (var member in type.Members)
            {
                WriteLine(
                    $"{FormatPropertyIdentifier(member.IdentifierName)} = {FormatParameterName(member.IdentifierName)};");
            }

            CloseBlock();
        }

        private void WriteLine(
            string text = "")
        {
            _builder.AppendLine(
                $"{new string(' ', _indentLevel * _indentSize)}{text}");
        }

        private void Indent()
            => _indentLevel++;

        private void Unindent()
        {
            if (_indentLevel <= 0) return;

            _indentLevel--;
        }

        private void OpenBlock()
        {
            WriteLine("{");
            Indent();
        }

        private void CloseBlock()
        {
            Unindent();
            WriteLine("}");
        }

        private static string FormatPropertyIdentifier(
            string name)
            => string.IsNullOrWhiteSpace(name)
                ? name
                : name.ToUpperCaseFirst();

        private static string FormatParameterName(
            string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return name;

            var result = name.ToLowerCaseFirst();
            return ReservedKeyword.IsKeyword(name)
                ? $"@{name}"
                : result;
        }
    }
}
