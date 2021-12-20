using System.Collections.Generic;

namespace TagsCloudContainer.TextPreparation
{
    public interface IFileReader
    {
        Result<List<string>> GetAllWords(string filePath);
    }
}