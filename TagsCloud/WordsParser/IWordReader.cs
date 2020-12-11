using System.Collections.Generic;
using TagsCloud.Result;

namespace TagsCloud.WordsParser
{
    public interface IWordReader
    {
        public Result<IEnumerable<string>> ReadWords();
    }
}