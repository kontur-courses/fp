using System.Collections.Generic;
using System.Linq;
using ResultOf;


namespace TagsCloudVisualization.Preprocessing
{
    public class DullWordsFilter : IFilter
    {
        private readonly DullWordsLoader wordsLoader;

        public DullWordsFilter(DullWordsLoader wordsLoader)
        {
            this.wordsLoader = wordsLoader;
        }

        public Result<IEnumerable<string>> FilterWords(IEnumerable<string> words)
        {
            return wordsLoader.LoadDullWords()
                .Then(dullWords => FilterWordsNotPure(words, dullWords.ToList()));
        }

        private IEnumerable<string> FilterWordsNotPure(IEnumerable<string> words, List<string> dullWords)
        {
            foreach (var word in words)
                if (!dullWords.Contains(word.ToLower()))
                    yield return word;
        }
    }
}
