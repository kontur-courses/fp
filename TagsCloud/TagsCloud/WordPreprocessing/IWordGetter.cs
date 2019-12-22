using System.Collections.Generic;
using TagsCloud.ErrorHandler;

namespace TagsCloud.WordPreprocessing
{
    public interface IWordGetter
    {
        Result<IEnumerable<string>> GetWords(params char[] delimiters);
    }
}