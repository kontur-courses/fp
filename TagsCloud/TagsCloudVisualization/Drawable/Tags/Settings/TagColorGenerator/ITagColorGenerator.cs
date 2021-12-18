using System.Drawing;
using TagsCloud.Utils;

namespace TagsCloudVisualization.Drawable.Tags.Settings.TagColorGenerator
{
    public interface ITagColorGenerator
    {
        Color Generate(Tag tag);
    }
}