namespace Pulse.CodeAnalysis.Helpers
{
    using System;
    using System.Globalization;
    using FrontEnd;

    internal static class ObjectFormatter
    {
        public static string Stringify(
            object? obj)
            => obj switch
            {
                null => Lexemes.Nil,
                IFormattable f => f.ToString(
                    null,
                    CultureInfo.InvariantCulture),
                _ => obj.ToString() ?? string.Empty,
            };
    }
}
