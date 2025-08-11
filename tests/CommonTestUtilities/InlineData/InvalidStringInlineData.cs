using System.Collections;

namespace CommonTestUtilities.InlineData
{
    public class InvalidStringInlineData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "" };
            yield return new object[] { "     " };
            yield return new object[] { null };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
