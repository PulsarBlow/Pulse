namespace Pulse.AstGenerator
{
    using System.Collections.Generic;

    internal class AstGenerationOptions
    {
        public AstKind AstKind { get; set; }
        public IEnumerable<string> TypeDescriptions { get; set; }
        public string BaseNamespace { get; set; }
        public string BaseTypeName { get; set; }
        public string FileName { get; set; }
        public string OutputDir { get; set; }
    }
}
