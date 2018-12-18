using System.Collections.Generic;
using ResultOf;


namespace TagsCloudVisualization.Preprocessing
{
    public class DullWordsFilter : IFilter
    {
        private readonly HashSet<string> dullWords;

        public DullWordsFilter(HashSet<string> dullWords)
        {
            this.dullWords = dullWords;
        }

        public IEnumerable<string> FilterWords(IEnumerable<string> words)
        {
            foreach (var word in words)
                if (!dullWords.Contains(word.ToLower()))
                    yield return word;
        }
    }
}
