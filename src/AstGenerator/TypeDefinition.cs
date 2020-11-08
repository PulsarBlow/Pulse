namespace Pulse.AstGenerator
{
    using System.Collections.Generic;
    using System.Linq;

    internal class TypeDefinition
    {
        private readonly List<MemberDefinition> _members =
            new List<MemberDefinition>();

        public string TypeName { get; }

        public IEnumerable<MemberDefinition> Members
            => _members.AsReadOnly();

        public TypeDefinition(
            string typeName,
            IEnumerable<MemberDefinition> fieldDefinitions = null)
        {
            TypeName = typeName;
            if (fieldDefinitions == null) { return; }

            var definitions = fieldDefinitions.ToList();
            _members.AddRange(definitions);
        }
    }
}
