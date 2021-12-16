using System.Collections.Generic;

namespace TagsCloudApp.WordsLoading
{
    public interface IWordsParser
    {
        IEnumerable<string> ParseText(string text);
    }
}