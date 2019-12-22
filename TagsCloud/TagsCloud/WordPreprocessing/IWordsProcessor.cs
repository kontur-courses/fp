using System.Collections.Generic;
using TagsCloud.ErrorHandler;

namespace TagsCloud.WordPreprocessing
{
    public interface IWordsProcessor
    {
        IEnumerable<Result<string>> ProcessWords(IEnumerable<string> words);
    }
}