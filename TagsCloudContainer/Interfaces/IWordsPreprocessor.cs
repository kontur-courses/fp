using System.Collections.Generic;

namespace TagsCloudContainer
{
    public interface IWordsPreprocessor
    {
        Result<Dictionary<string, int>> GetWordsFrequency();
    }
}
