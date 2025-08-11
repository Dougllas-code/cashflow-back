using System.Collections;

namespace CommonTestUtilities.InlineData
{
    public class CultureInlineData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "en" };
            yield return new object[] { "pt-PT" };
            yield return new object[] { "pt-BR" };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
