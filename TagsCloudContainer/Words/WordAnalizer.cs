using System.Linq;
using ResultOf;

namespace TagsCloudContainer.Words
{
    public class WordAnalizer
    {
        public Result<WordPack[]> WordPacks { get; }

        public WordAnalizer(WordPreprocessing wordPreprocessing)
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
