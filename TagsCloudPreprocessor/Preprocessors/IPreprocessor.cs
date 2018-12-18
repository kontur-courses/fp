using System.Collections.Generic;
using ResultOfTask;

namespace TagsCloudPreprocessor.Preprocessors
{
    public interface IPreprocessor
    {
        Result<List<string>> PreprocessWords(List<string> words);
    }
}