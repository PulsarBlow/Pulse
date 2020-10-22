namespace Pulse.AstGenerator.Tests
{
    using AstGenerator;
    using Xunit;

    public class StringExtensionsTest
    {
        [Theory]
        [InlineData(
            null,
            null)]
        [InlineData(
            "",
            "")]
        [InlineData(
            "hello",
            "Hello")]
        [InlineData(
            "helloWorld",
            "HelloWorld")]
        public void ToUpperCaseFist_Returns_Expected(
            string value,
            string expected)
        {
            Assert.Equal(
                expected,
                value.ToUpperCaseFirst());
        }

        [Theory]
        [InlineData(
            null,
            null)]
        [InlineData(
            "",
            "")]
        [InlineData(
            "Hello",
            "hello")]
        [InlineData(
            "HelloWorld",
            "helloWorld")]
        public void ToLowerCaseFist_Returns_Expected(
            string value,
            string expected)
        {
            Assert.Equal(
                expected,
                value.ToLowerCaseFirst());
        }
    }
}
