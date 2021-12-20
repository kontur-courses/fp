using System.Collections.Generic;
using System.IO;

namespace TagsCloudVisualization.WordsProvider.FileReader
{
    internal class TxtFileReader : IWordsReader
    {
        public bool IsSupportedFileExtension(string extension) => extension == ".txt";

        public IEnumerable<string> GetFileContent(string path) => File.ReadLines(path);
    }
}