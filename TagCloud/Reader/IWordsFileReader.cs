using System.Collections.Generic;
using TagCloud.Data;

namespace TagCloud.Reader
{
    public interface IWordsFileReader
    {
        Result<IEnumerable<string>> ReadWords(string fileName);
    }
}