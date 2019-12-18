using System.Collections.Generic;
using System.Linq;

namespace TagCloud.WordsPreprocessing.TextAnalyzers
{
    [Name("DefaultAnalyzer")]
    public class DefaultAnalyzer : ITextAnalyzer
    {
        public Result<Word[]> GetWords(IEnumerable<string> words, int count)
        {
            return Result.Ok(words.Select(w => new Word(w, SpeechPart.Noun){ Frequency = 1}).Take(count).ToArray());
        }
    }
}