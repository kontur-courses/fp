using TagsCloudContainer.Tags;

namespace TagsCloudContainer.TagPainters;

public interface ITagPainter
{
    string Name { get; }

    IEnumerable<PaintedTag> PaintTags(IEnumerable<Tag> tags);
}
