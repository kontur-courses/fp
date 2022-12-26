using TagCloudContainer.Core.Models;
using TagCloudContainer.Core.Interfaces;

namespace TagCloudContainer.Configs;

public class SelectedValues : ISelectedValues
{
    public bool PlaceWordsRandomly { get; set; }
    public Size ImageSize { get; set; }
    public string FontFamily { get; set; }
    public Color WordsColor { get; set; }
    public Color BackgroundColor { get; set; }

    public SelectedValues()
    {
        var firstAvailableColor = Colors.GetAll().First().Value;
        WordsColor = BackgroundColor = firstAvailableColor;
    }
}