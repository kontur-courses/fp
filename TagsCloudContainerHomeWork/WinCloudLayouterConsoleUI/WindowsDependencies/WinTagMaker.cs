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
[SuppressMessage("ReSharper", "ConstantNullCoalescingCondition")]
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

            if (!tagSize.IsSuccess)
            {
                return ResultExtension.Fail<TagToRender>($"Ошибка при получении локации тега\n{tagSize.Error}");
            }

            var putRectangle = layouter.PutNextRectangle(tagSize.GetValueOrThrow());

            if (!putRectangle.IsSuccess)
            {
                return ResultExtension.Fail<TagToRender>($"Ошибка при получении локации тега\n{putRectangle.Error}");
            }

            var color = int.Parse("FF" + settings.FontColor, NumberStyles.HexNumber);
            var limRectangle = putRectangle.GetValueOrThrow();

            return new TagToRender(limRectangle, raw.Key, color, fontSize.GetValueOrThrow(), settings.FontName);
        }
        catch (Exception e)
        {
            return ResultExtension.Fail<TagToRender>($"Ошибка при создании тега\n{e.GetType().Name}: {e.Message}");
        }
    }

    private Result<float> GetFontSize(KeyValuePair<string, int> tag, IStatisticMaker statMaker)
    {
        var leastTag = statMaker.GetLeastFrequentTag();
        var mostTag = statMaker.GetMostFrequentTag();

        if (!leastTag.IsSuccess || !mostTag.IsSuccess)
        {
            var error = string.Join("\n", leastTag.Error ?? "", mostTag.Error ?? "");
            return ResultExtension.Fail<float>(error);
        }

        var size = settings.FontMinSize + settings.FontMaxSize * (tag.Value - leastTag.GetValueOrThrow().Value) /
            (mostTag.GetValueOrThrow().Value -
             leastTag.GetValueOrThrow().Value);

        return size >= settings.FontMinSize
            ? size
            : settings.FontMinSize;
    }

    private static Result<Size> GetTagSize(string tag, string fontName, Result<float> fontSize, Size imageSize)
    {
        if (!fontSize.IsSuccess)
        {
            return ResultExtension.Fail<Size>(fontSize.Error);
        }

        using var mockImage = new Bitmap(imageSize.Width, imageSize.Height);
        using var mockGraphics = Graphics.FromImage(mockImage);
        using var mockFont = new Font(fontName, fontSize.GetValueOrThrow());

        return mockGraphics.MeasureString(tag, mockFont).ToSize();
    }
}