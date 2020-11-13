namespace Pulse.AstGenerator
{
    using System.Collections.Generic;
    using System.Linq;

    internal class TypeDescriptor
    {
        private readonly List<MemberDescriptor> _members =
            new List<MemberDescriptor>();

        public string TypeName { get; }

        public IEnumerable<MemberDescriptor> Members
            => _members.AsReadOnly();

        public TypeDescriptor(
            string typeName,
            IEnumerable<MemberDescriptor> members = null)
        {
            TypeName = typeName;
            if (members == null) { return; }

            var definitions = members.ToList();
            _members.AddRange(definitions);
        }
    }
}
