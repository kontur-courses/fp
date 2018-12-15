using ResultOfTask;

namespace TagsCloudPreprocessor
{
    public interface IFileReader
    {
        Result<string> ReadFromFile(string path);
    }
}