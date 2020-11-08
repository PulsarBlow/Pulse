namespace Pulse.CodeAnalysis.Tests.Helpers
{
    using System.Collections.Generic;
    using CodeAnalysis.FrontEnd;
    using CodeAnalysis.Helpers;
    using Xunit;

    public class ObjectFormatterTest
    {
        [Theory]
        [MemberData(nameof(Stringify_Valid_TestCases))]
        public void Stringify_Returns_Expected_String(
            object value,
            string expected)
        {
            var result = ObjectFormatter.Stringify(value);

            Assert.Equal(
                expected,
                result);
        }

        public static IEnumerable<object[]> Stringify_Valid_TestCases()
        {
            // null -> "nil"
            yield return new object[]
            {
                null,
                Lexemes.Nil,
            };

            // 1D -> "1"
            yield return new object[]
            {
                1D,
                "1",
            };

            // 1.1 -> "1.1"
            yield return new object[]
            {
                1.1D,
                "1.1",
            };

            // 10000 -> "10000"
            yield return new object[]
            {
                10000,
                "10000",
            };

            // "a" -> "a"
            yield return new object[]
            {
                "a",
                "a",
            };
        }
    }
}
