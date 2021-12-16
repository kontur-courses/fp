using System.Collections.Generic;
using TagsCloudContainer.Results;

namespace TagsCloudContainer.Preprocessing
{
    public interface IWordsPreprocessor
    {
        Result<IEnumerable<string>> Preprocess(IEnumerable<string> words);
    }
}