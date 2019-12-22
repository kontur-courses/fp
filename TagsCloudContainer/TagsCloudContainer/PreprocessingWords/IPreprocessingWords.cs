using System.Collections.Generic;
using ResultOf;

namespace TagsCloudContainer.PreprocessingWords
{
    public interface IPreprocessingWords
    {
        Result<IEnumerable<string>> Preprocessing(IEnumerable<string> strings);
    }
}