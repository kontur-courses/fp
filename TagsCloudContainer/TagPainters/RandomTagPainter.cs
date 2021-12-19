using TagsCloudContainer.Infrastructure.Tags;

namespace TagsCloudContainer.TagPainters;

public class RandomTagPainter : ITagPainter
{
    public string Name => "Random";
    private readonly Random rnd;

    public RandomTagPainter()
    {
        rnd = new Random();
    }

    public IEnumerable<PaintedTag> PaintTags(IEnumerable<Tag> tags)
    {
        foreach (var tag in tags)
        {
            var randomColor = Color.FromArgb(rnd.Next(256),
                rnd.Next(256), rnd.Next(256));
            yield return new PaintedTag(tag, randomColor);
        }
    }
}