using System.Collections.Generic;

namespace TagsCloudContainer.TextPreparation
{
    public interface IWordsHelper
    {
        Result<List<string>> GetAllWordsToVisualize(List<string> words);
    }
}