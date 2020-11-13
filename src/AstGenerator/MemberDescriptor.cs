namespace Pulse.AstGenerator
{
    internal class MemberDescriptor
    {
        public string TypeName { get; }
        public string IdentifierName { get; }

        public MemberDescriptor(
            string typeName,
            string identifierName)
        {
            TypeName = typeName;
            IdentifierName = identifierName;
        }
    }
}
