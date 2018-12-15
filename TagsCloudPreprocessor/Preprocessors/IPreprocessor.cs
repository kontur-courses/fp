using System.Collections.Generic;
using ResultOfTask;

namespace TagsCloudPreprocessor.Preprocessors
{
    public interface IPreprocessor
    {
        Result<List<string>> PreprocessWords(Result<List<string>> words);
    }
}