using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Globalization;
using TagsCloudContainerCore;
using TagsCloudContainerCore.CircularLayouter;
using TagsCloudContainerCore.InterfacesCore;
using TagsCloudContainerCore.LayoutSettingsDir;
using TagsCloudContainerCore.StatisticMaker;

namespace WinCloudLayouterConsoleUI.WindowsDependencies;

[SuppressMessage("Interoperability", "CA1416", MessageId = "Проверка совместимости платформы")]
public class WinTagMaker : ITagMaker
{
    private readonly ILayouter layouter;
    private readonly LayoutSettings settings;

    public WinTagMaker(ILayouter layouter, LayoutSettings settings)
    {
        this.layouter = layouter;
        this.settings = settings;
    }

    public TagToRender MakeTag(KeyValuePair<string, int> raw, IStatisticMaker statisticMaker)
    {
        var fontSize = GetFontSize(raw, statisticMaker);
        var tagSize = GetTagSize(raw.Key, settings.FontName, fontSize, settings.PictureSize);
        var putRectangle = layouter.PutNextRectangle(tagSize);
        var color = int.Parse("FF" + settings.FontColor, NumberStyles.HexNumber);
        var location = putRectangle.GetValueOrThrow().Location;

        return new TagToRender(location, raw.Key, color, fontSize, settings.FontName);
    }

    private float GetFontSize(KeyValuePair<string, int> tag, IStatisticMaker statMaker)
    {
        var size = settings.FontMinSize + settings.FontMaxSize * (tag.Value - statMaker.GetLeastFrequentTag().Value) /
            (statMaker.GetMostFrequentTag().Value -
             statMaker.GetLeastFrequentTag().Value);
        return size >= settings.FontMinSize
            ? size
            : settings.FontMinSize;
    }

    private static Size GetTagSize(string tag, string fontName, float fontSize, Size imageSize)
    {
        using var mockImage = new Bitmap(imageSize.Width, imageSize.Height);
        using var mockGraphics = Graphics.FromImage(mockImage);
        using var mockFont = new Font(fontName, fontSize);

        return mockGraphics.MeasureString(tag, mockFont).ToSize();
    }
}