using System.Collections;

namespace CommonTestUtilities.InlineData
{
    public class InvalidPasswordInlineData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "" };
            yield return new object[] { "      " };
            yield return new object[] { null };
            yield return new object[] { "a" };
            yield return new object[] { "aa" };
            yield return new object[] { "aaa" };
            yield return new object[] { "aaaa" };
            yield return new object[] { "aaaaa" };
            yield return new object[] { "aaaaaa" };
            yield return new object[] { "aaaaaaa" };
            yield return new object[] { "aaaaaaaa" };
            yield return new object[] { "AAAAAAAA" };
            yield return new object[] { "Aaaaaaaa" };
            yield return new object[] { "Aaaaaaa1" };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
