using ResultOf;

namespace TagsCloudVisualization.WordsFileReading
{
    public interface IFileReader
    {
        Result<string> ReadText(string fileName);
        string[] SupportedTypes();
    }
}
