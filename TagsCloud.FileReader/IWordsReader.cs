using System.Collections.Generic;
using TagsCloud.ResultPattern;

namespace TagsCloud.FileReader
{
    public interface IWordsReader
    {
        Result<List<string>> ReadWords(string path);
    }
}