using ResultOf;

namespace TagCloud.TextProcessing
{
    public interface IFileProvider
    {
        Result<string> GetTxtFilePath(string path);
    }
}