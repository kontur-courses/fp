using System.Collections.Generic;

namespace TagsCloudContainer.Processing
{
    public interface IParser
    {
        Dictionary<string, int> ParseWords(string input);
    }
}