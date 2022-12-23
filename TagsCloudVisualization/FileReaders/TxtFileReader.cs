using System.IO;
using ResultOf;

namespace TagsCloudVisualization.FileReaders
{
    public class TxtFileReader : IFileReader
    {
        public TxtFileReader(string filePath)
        {
            FilePath = filePath;
        }

        public string FilePath { get; }

        public bool TryReadAllText(out string text)
        {
            var resultText = Result.Of(() => File.ReadAllText(FilePath));
            text = resultText.IsSuccess ? resultText.Value : string.Empty;
            return resultText.IsSuccess;
        }
    }
}