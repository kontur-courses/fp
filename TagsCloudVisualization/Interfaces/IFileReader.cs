using System.Collections.Generic;

namespace TagsCloudVisualization.Interfaces
{
    public interface IFileReader
    {
        Result<List<string>> GetWordsFromFile(string path, char[] separators);
    }
}