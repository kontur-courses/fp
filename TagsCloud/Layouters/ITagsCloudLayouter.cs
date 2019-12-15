using System.Collections.Immutable;

namespace TagsCloud.Layouters
{
    public interface ITagsCloudLayouter
    {
        Result<ImmutableList<LayoutItem>> ReallocItems(ImmutableList<LayoutItem> items);
    }
}
