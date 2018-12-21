using System.Collections.Generic;
using TagsCloudContainer.Settings;

namespace TagsCloudContainer.Processing
{
    public interface IParser
    {
        ParserSettings Settings { get; }
        Dictionary<string, int> ParseWords(string input);
    }
}