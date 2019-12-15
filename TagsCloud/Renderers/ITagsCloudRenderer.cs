using System.Collections.Immutable;
using System.Drawing;
using TagsCloud.Layouters;

namespace TagsCloud.Renderers
{
    public interface ITagsCloudRenderer
    {
        Font TagFont { get; set; }
        int ImageWidth { get; set; }
        int ImageHeight { get; set; }
        Result<ImmutableList<LayoutItem>> CalcTagsRectanglesSizes(ImmutableList<LayoutItem> layoutItems);
        Result<Image> Render(ImmutableList<LayoutItem> layoutItems);
    }
}
