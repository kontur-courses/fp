using System.Collections.Generic;
using TagsCloud.Visualization.Utils;

namespace TagsCloud.Visualization.WordsParser
{
    public interface IWordsParser
    {
        Result<Dictionary<string, int>> CountWordsFrequency(string text);
    }
}