using System.Linq;
using ResultOf;

namespace TagsCloudContainer.Words
{
    public class WordAnalyzer : IWordAnalyzer
    {
        public Result<WordPack[]> WordPacks { get; }

        public WordAnalyzer(WordPreprocessing wordPreprocessing)
        {
            WordPacks = wordPreprocessing
                .Words
                .Then(words => words
                    .GroupBy(w => w)
                    .Select(g => new WordPack(g.Key, g.Count()))
                    .ToArray())
                .RefineError("Word analyze fail");
        }
    }
}
