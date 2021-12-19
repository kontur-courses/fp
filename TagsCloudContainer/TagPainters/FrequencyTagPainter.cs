using TagsCloudContainer.Infrastructure.Settings;
using TagsCloudContainer.Infrastructure.Tags;
using TagsCloudContainer.Interfaces;

namespace TagsCloudContainer.TagPainters;

public class FrequencyTagPainter : ITagPainter
{
    public string Name => "Frequency";
    private readonly Settings settings;

    public FrequencyTagPainter(Settings settings)
    {
        this.settings = settings;
    }

    public IEnumerable<PaintedTag> PaintTags(IEnumerable<Tag> tags)
    {
        foreach (var tag in tags)
        {
            var alpha = ((int)(255 * tag.Frequency) + 100) % 255;
            var cl = Color.FromArgb(alpha, settings.Palette.Primary);
            yield return new PaintedTag(tag, cl);
        }
    }
}