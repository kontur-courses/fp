using System.Linq;

namespace TagsCloudResult.TextPreprocessors.Filters
{
    class BoringWordFilter : IWordFilter
    {
        private readonly string[] boringWords = { "который", "большой" };

        public bool Filter(string word)
        {
            return !boringWords.Contains(word);
        }
    }
}