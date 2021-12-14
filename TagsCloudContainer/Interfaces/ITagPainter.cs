using TagsCloudContainer.Infrastructure;

namespace TagsCloudContainer.Interfaces;

public interface ITagPainter
{
    string Name { get; }

    IEnumerable<PaintedTag> Paint(IEnumerable<Tag> tags);
}
