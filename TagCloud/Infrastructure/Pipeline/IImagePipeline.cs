using TagCloud.Infrastructure.Pipeline.Common;

namespace TagCloud.Infrastructure.Pipeline;

public interface IImagePipeline
{
    public void Run(IAppSettings settings);
}