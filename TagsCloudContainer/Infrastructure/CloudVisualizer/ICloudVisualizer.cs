using ResultOf;

namespace TagsCloudContainer.Infrastructure.CloudVisualizer
{
    internal interface ICloudVisualizer
    {
        public Result<None> Visualize();
    }
}