using ResultOf;

namespace TagCloudCreator
{
    public interface IFileReader
    {
        string[] Types { get; }
        public Result<string[]> ReadAllLinesFromFile(string path);
    }
}