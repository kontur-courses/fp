using System.Collections.Generic;
using TagsCloud.Result;

namespace TagsCloud.WordsParser
{
    public interface IFilter
    {
        public Result<IEnumerable<string>> RemoveBoringWords(IEnumerable<string> words);
    }
}