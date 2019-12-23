using System.Collections.Generic;
using TagsCloud.ErrorHandler;

namespace TagsCloud.WordPreprocessing
{
    public interface IWordsProcessor
    {
        Result<IEnumerable<string>> ProcessWords(IEnumerable<string> words);
    }
}