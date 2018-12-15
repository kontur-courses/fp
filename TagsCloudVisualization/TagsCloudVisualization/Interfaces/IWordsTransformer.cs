using System.Collections.Generic;

namespace TagsCloudVisualization.Interfaces
{
    public interface IWordsTransformer
    {
        Result<List<string>> GetStems(List<string> words);
    }
}