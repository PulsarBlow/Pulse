namespace Pulse.CodeAnalysis.Helpers
{
    internal static class ObjectComparer
    {
        public static bool AreEqual(
            object? a,
            object? b)
        {
            return a switch
            {
                null when b == null => true,
                null => false,
                _ => a.Equals(b),
            };
        }

        public static bool IsTruthy(
            object? obj)
        {
            // false and nil are falsey and everything else is truthy
            // eg. "" -> True, 0 -> True, -1 -> True, ...
            return obj switch
            {
                null => false,
                bool value => value,
                _ => true,
            };
        }
    }
}
