using CSharpFunctionalExtensions;

namespace TagsCloudContainer.Input
{
    public interface IFileReader
    {
        Result<string> Read(string path);
    }
}