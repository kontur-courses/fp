using System.Collections.Generic;

namespace TagsCloudContainer.Preprocessing
{
    public interface IWordsPreprocessor
    {
        OperationResult<IEnumerable<string>> Process(IEnumerable<string> words);
    }
}