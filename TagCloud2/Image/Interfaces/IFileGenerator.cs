using ResultOf;

namespace TagCloud2
{
    public interface IFileGenerator
    {
        Result<None> GenerateFile(string name, IImageFormatter formatter, System.Drawing.Image image);
    }
}
