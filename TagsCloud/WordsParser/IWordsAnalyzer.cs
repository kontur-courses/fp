using System.Collections.Generic;
using TagsCloud.Result;

namespace TagsCloud.WordsParser
{
    public interface IWordsAnalyzer
    {
        public Result<Dictionary<string, int>> AnalyzeWords();
    }
}