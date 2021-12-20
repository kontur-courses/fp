using System.Collections.Generic;

namespace TagsCloudVisualization.WordsProvider.FileReader
{
    public interface IWordsReader
    {
        bool IsSupportedFileExtension(string extension);
        IEnumerable<string> GetFileContent(string path);
    }
}