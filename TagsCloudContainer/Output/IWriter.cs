using CSharpFunctionalExtensions;

namespace TagsCloudContainer.Output
{
    public interface IWriter
    {
        Result WriteToFile(byte[] bytes, string path);
    }
}