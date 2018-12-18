using System.Collections.Generic;
using ResultOf;

namespace TagsCloudContainer.Preprocessing
{
    public interface IWordsPreprocessor
    {
        Result<IEnumerable<string>> Process(IEnumerable<string> words);
    }
}