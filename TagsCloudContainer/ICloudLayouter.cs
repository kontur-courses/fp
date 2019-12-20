using TagsCloudContainer.ResultInfrastructure;

namespace TagsCloudContainer
{
    public interface ICloudLayouter
    {
        Result<None> Layout(string inputPath, string outputPath);
    }
}