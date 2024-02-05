namespace TagsCloudVisualization.Settings;

public class TagsLayouterSettings
{
    public string FontFamily { get; }
    public int MinSize { get; }
    public int MaxSize { get; }

    public TagsLayouterSettings(string font, int minSize, int maxSize)
    {
        FontFamily = font;
        MinSize = minSize;
        MaxSize = maxSize;
    }
}