using ResultOf;
using Syncfusion.DocIO.DLS;

namespace TagsCloudVisualization.FileReaders
{
    public class DocFileReader : IFileReader
    {
        public DocFileReader(string path)
        {
            FilePath = path;
        }

        public string FilePath { get; }

        public bool TryReadAllText(out string text)
        {
            var resultText = Result.Of(() => new WordDocument(FilePath).GetText());
            text = resultText.IsSuccess ? resultText.Value : string.Empty;
            return resultText.IsSuccess;
        }
    }
}