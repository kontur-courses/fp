using TagsCloudContainer.Infrastructure.Tags;

namespace TagsCloudContainer.TagPainters;

public interface ITagPainter
{
    string Name { get; }

    IEnumerable<PaintedTag> PaintTags(IEnumerable<Tag> tags);
}
