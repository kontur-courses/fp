using System.Collections.Generic;

namespace TagCloud.WordsPreprocessing.TextAnalyzers
{
    public interface ITextAnalyzer
    {
        Result<Word[]> GetWords(IEnumerable<string> words, int count);
    }
}