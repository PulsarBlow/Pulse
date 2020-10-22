namespace Pulse.AstGenerator
{
    public class MemberDefinition
    {
        public string TypeName { get; }
        public string IdentifierName { get; }

        public MemberDefinition(
            string typeName,
            string identifierName)
        {
            TypeName = typeName;
            IdentifierName = identifierName;
        }
    }
}
