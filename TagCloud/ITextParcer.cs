using System.Collections.Generic;
using ResultOf;

namespace TagCloud
{
    public interface ITextParcer
    {
        Result<List<string>> TryGetWordsFromText(string input);
        Result<Dictionary<string, int>> ParseWords(List<string> words);
    }
}