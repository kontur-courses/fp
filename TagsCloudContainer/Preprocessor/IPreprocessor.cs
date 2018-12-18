using System.Collections.Generic;
using TagsCloudContainer.ResultOf;

namespace TagsCloudContainer.Preprocessor
{
    public interface IPreprocessor
    {
        Result<IEnumerable<string>> PrepareWords(IEnumerable<string> words);
    }
}