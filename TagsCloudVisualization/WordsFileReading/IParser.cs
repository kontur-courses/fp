using System.Collections.Generic;
using ResultOf;

namespace TagsCloudVisualization.WordsFileReading
{
    public interface IParser
    {
        Result<IEnumerable<string>> ParseText(string text);
        string GetModeName();
    }
}
