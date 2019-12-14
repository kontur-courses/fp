using System.Collections.Generic;

namespace TagsCloudResult.TextParsing
{
    public interface IFileWordsParser
    {
        IEnumerable<string> ParseFrom(string path);
    }
}