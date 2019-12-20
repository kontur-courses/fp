using TagsCloudContainer.ResultInfrastructure;

namespace TagsCloudContainer
{
    public interface ICloudLayouter
    {
        Result Layout(string inputPath, string outputPath);
    }
}