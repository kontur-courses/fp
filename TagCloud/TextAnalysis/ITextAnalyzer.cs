using System.Collections.Generic;
using ResultOf;

namespace TagCloud
{
    public interface ITextAnalyzer
    {
        Result<IEnumerable<Word>> GetWordList();
        Result<int> GetMaxFrequency();
    }
}