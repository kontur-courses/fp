using System.IO;
using ResultOf;

namespace TagCloudCreator
{
    public class TxtFileReader : IFileReader
    {
        public string[] Types { get; } = {".txt"};

        public Result<string[]> ReadAllLinesFromFile(string path)
        {
            return File.ReadAllLines(path);
        }
    }
}