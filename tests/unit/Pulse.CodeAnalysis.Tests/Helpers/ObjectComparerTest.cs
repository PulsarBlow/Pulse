namespace Pulse.CodeAnalysis.Tests.Helpers
{
    using System.Collections.Generic;
    using CodeAnalysis.Helpers;
    using Xunit;

    public class ObjectComparerTest
    {
        [Theory]
        [MemberData(nameof(AreEqual_Test_Cases))]
        public void AreEqual_Returns_Expected_Result(
            object a,
            object b,
            bool expected)
        {
            var actual = ObjectComparer.AreEqual(
                a,
                b);

            Assert.Equal(
                expected,
                actual);
        }

        [Theory]
        [MemberData(nameof(IsTruthy_Test_Cases))]
        public void IsTruthy_Returns_Expected_Result(
            object value,
            bool expected)
        {
            var actual = ObjectComparer.IsTruthy(value);

            Assert.Equal(
                expected,
                actual);
        }

        public static IEnumerable<object[]> AreEqual_Test_Cases()
        {
            // 1 == nil -> False
            yield return new object[]
            {
                1D,
                null,
                false,
            };

            // nil == 1 -> False
            yield return new object[]
            {
                null,
                1D,
                false,
            };

            // nil == nil -> True
            yield return new object[]
            {
                null,
                null,
                true,
            };

            // 1 == 1 -> True
            yield return new object[]
            {
                1D,
                1D,
                true,
            };

            // true == true -> True
            yield return new object[]
            {
                true,
                true,
                true,
            };

            // false == true -> False
            yield return new object[]
            {
                false,
                true,
                false,
            };
        }

        public static IEnumerable<object[]> IsTruthy_Test_Cases()
        {
            // nil -> False
            yield return new object[]
            {
                null,
                false,
            };

            // false -> False
            yield return new object[]
            {
                false,
                false,
            };

            // 0 -> True
            yield return new object[]
            {
                0,
                true,
            };

            // 1 -> True
            yield return new object[]
            {
                1D,
                true,
            };

            // true -> True
            yield return new object[]
            {
                true,
                true,
            };

            // "" -> True
            yield return new object[]
            {
                string.Empty,
                true,
            };

            // "a" -> True
            yield return new object[]
            {
                "a",
                true,
            };

            // "0" -> True
            yield return new object[]
            {
                "0",
                true,
            };
        }
    }
}
