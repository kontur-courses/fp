namespace TagsCloudContainer.Infrastructure.Tags;

public record PaintedTag : Tag
{
    public Color Color { get; init; }

    public PaintedTag(Tag original, Color color) : base(original)
    {
        Color = color;
    }
}
