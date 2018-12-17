using System.Collections.Generic;
using System.Linq;
using ResultOf;

namespace TagsCloudVisualization.WordsProcessing
{
    public class WordsChanger : IWordsChanger
    {
        public Result<IEnumerable<string>> ChangeWords(IEnumerable<string> words)
        {
            return Result.Of(() => words.Select(w => w.ToLower()));
        }
    }
}