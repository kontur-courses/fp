using System.Collections.Generic;

namespace TagsCloudVisualization.Interfaces
{
    public interface IFileReader
    {
        Result<IEnumerable<string>> GetWordsFromFile(string path, char[] separators);
    }
}