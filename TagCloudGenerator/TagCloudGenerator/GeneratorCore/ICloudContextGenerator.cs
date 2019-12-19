using TagCloudGenerator.ResultPattern;

namespace TagCloudGenerator.GeneratorCore
{
    public interface ICloudContextGenerator
    {
        Result<TagCloudContext> GetTagCloudContext();
    }
}