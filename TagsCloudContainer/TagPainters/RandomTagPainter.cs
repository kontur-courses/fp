using TagsCloudContainer.Infrastructure.Tags;
using TagsCloudContainer.Interfaces;

namespace TagsCloudContainer.TagPainters;

public class RandomTagPainter : ITagPainter
{
    public string Name => "Random";
    private readonly Random rnd;

    public RandomTagPainter()
    {
        rnd = new Random();
    }

    public IEnumerable<PaintedTag> Paint(IEnumerable<Tag> tags)
    {
        foreach (var tag in tags)
        {
            var randomColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
            var cl = randomColor;
            yield return new PaintedTag(tag, cl);
        }
    }
}