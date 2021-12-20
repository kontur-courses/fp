using System.Collections.Generic;

namespace TagsCloudContainer.TextPreparation
{
    public interface IWordsReader
    {
        Result<List<string>> ReadAllWords(string fileContent);
    }
}