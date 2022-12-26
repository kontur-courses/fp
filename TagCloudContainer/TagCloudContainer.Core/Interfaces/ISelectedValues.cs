namespace TagCloudContainer.Core.Interfaces;

public interface ISelectedValues
{
    public bool PlaceWordsRandomly { get; set; }
    public Size ImageSize { get; set; }
    public string FontFamily { get; set; }
    public Color WordsColor { get; set; }
    public Color BackgroundColor { get; set; }
}