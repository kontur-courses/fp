using System.Drawing;
using TagsCloud.Visualization.TextProviders;
using TagsCloud.Visualization.Utils;

namespace TagsCloud.Visualization.TagsCloudVisualizers
{
    public interface ITagsCloudVisualizer
    {
        Result<Image> GenerateImage(ITextProvider textProvider);
    }
}