#region

using System.Collections.Generic;

#endregion

namespace TagsCloudVisualization.Interfaces
{
    public interface IFileReader
    {
        Result<IEnumerable<string>> GetWordsFromFile(string path, char[] separators);
    }
}