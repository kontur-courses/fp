using System.Drawing;
using TagsCloudPainter.Tags;

namespace TagsCloudPainter;

public class TagsCloud
{
    public TagsCloud(Point center, List<(Tag, Rectangle)> tags)
    {
        Center = center;
        Tags = tags ?? [];
    }

    public Point Center { get; private set; }
    public List<(Tag, Rectangle)> Tags { get; }

    public void AddTag(Tag tag, Rectangle rectangle)
    {
        Tags.Add((tag, rectangle));
    }

    public int GetWidth()
    {
        if (Tags.Count == 1)
            return Tags.First().Item2.Width;

        return Tags.Max(pair => pair.Item2.X) - Tags.Min(pair => pair.Item2.X);
    }

    public int GetHeight()
    {
        if (Tags.Count == 1)
            return Tags.First().Item2.Height;

        return Tags.Max(pair => pair.Item2.Y) - Tags.Min(pair => pair.Item2.Y);
    }
}