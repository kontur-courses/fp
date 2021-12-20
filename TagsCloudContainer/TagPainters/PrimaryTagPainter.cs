using TagsCloudContainer.Infrastructure;
using TagsCloudContainer.Tags;

namespace TagsCloudContainer.TagPainters;

public class PrimaryTagPainter : ITagPainter
{
    public string Name => "Primary";
    private readonly Settings settings;

    public PrimaryTagPainter(Settings settings)
    {
        this.settings = settings;
    }

    public IEnumerable<PaintedTag> PaintTags(IEnumerable<Tag> tags)
    {
        return tags.Select(tag => new PaintedTag(tag, settings.Palette.Primary));
    }
}