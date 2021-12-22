using System.Collections.Generic;
using TagsCloud.Visualization.Models;
using TagsCloud.Visualization.Utils;

namespace TagsCloud.Visualization.WordsParsers
{
    public interface IWordsParser
    {
        Result<IEnumerable<Word>> CountWordsFrequency(string text);
    }
}