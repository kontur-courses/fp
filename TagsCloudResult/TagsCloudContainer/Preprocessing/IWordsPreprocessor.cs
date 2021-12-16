using System.Collections.Generic;

namespace TagsCloudContainer.Preprocessing
{
    public interface IWordsPreprocessor
    {
        Result<IEnumerable<string>> Preprocess(IEnumerable<string> words);
    }
}