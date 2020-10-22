namespace Pulse.AstGenerator
{
    public static class StringExtensions
    {
        public static string ToUpperCaseFirst(
            this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;

            var array = value.ToCharArray();
            array[0] = char.ToUpper(array[0]);
            return new string(array);
        }

        public static string ToLowerCaseFirst(
            this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;

            var array = value.ToCharArray();
            array[0] = char.ToLower(array[0]);
            return new string(array);
        }
    }
}
