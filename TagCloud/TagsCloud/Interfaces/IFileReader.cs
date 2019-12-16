using TagsCloud.ErrorHandling;

namespace TagsCloud.Interfaces
{
    public interface ITextReader
    {
        Result<string> ReadFile(string path);
    }
}