using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Globalization;
using TagsCloudContainerCore;
using TagsCloudContainerCore.CircularLayouter;
using TagsCloudContainerCore.InterfacesCore;
using TagsCloudContainerCore.LayoutSettingsDir;
using TagsCloudContainerCore.Result;
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

    public Result<TagToRender> MakeTag(KeyValuePair<string, int> raw, IStatisticMaker statisticMaker)
    {
        try
        {
            var fontSize = GetFontSize(raw, statisticMaker);
            var tagSize = GetTagSize(raw.Key, settings.FontName, fontSize, settings.PictureSize);
            var putRectangle = layouter.PutNextRectangle(tagSize);

            if (!putRectangle.IsSuccess)
            {
                return ResultExtension.Fail<TagToRender>($"Ошибка при получении локации тега\n{putRectangle.Error}");
            }

            var color = int.Parse("FF" + settings.FontColor, NumberStyles.HexNumber);
            var limRectangle = putRectangle.GetValueOrThrow();

            return new TagToRender(limRectangle, raw.Key, color, fontSize, settings.FontName);
        }
        catch (Exception e)
        {
            return ResultExtension.Fail<TagToRender>($"Ошибка при создании тега\n{e.GetType().Name}: {e.Message}");
        }
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