using TagsCloudContainer.Infrastructure.Settings;
using TagsCloudContainer.Infrastructure.Tags;
using TagsCloudContainer.Interfaces;

namespace TagsCloudContainer.TagPainters;

public class PrimaryTagPainter : ITagPainter
{
    public string Name => "Primary";
    private readonly Settings settings;

    public PrimaryTagPainter(Settings settings)
    {
        this.settings = settings;
    }

    public IEnumerable<PaintedTag> Paint(IEnumerable<Tag> tags)
    {
        foreach (var tag in tags)
            yield return new PaintedTag(tag, settings.Palette.Primary);
    }
}