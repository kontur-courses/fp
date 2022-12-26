using System.Drawing;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using TagsCloudContainer.Interfaces;

namespace TagsCloudContainer;

public class ClassicDrawer : IDrawer
{
    private readonly ILayouterAlgorithmProvider algorithmProvider;
    private readonly ClassicDrawerSettings classicDrawerSettings;
    private readonly Graphics graphics;

    public ClassicDrawer(ClassicDrawerSettings classicDrawerSettings, Graphics graphics,
        ILayouterAlgorithmProvider algorithmProvider)
    {
        this.classicDrawerSettings = classicDrawerSettings;
        this.graphics = graphics;
        this.algorithmProvider = algorithmProvider;
    }

    public Result DrawCloud(IEnumerable<CloudWord> cloudWords)
    {
        return algorithmProvider.Provide()
            .Bind((Func<ILayouterAlgorithm, Result<IEnumerable<TagDrawingItem>>>)(algorithm =>
                cloudWords.GetTagDrawingItems(algorithm, GetFont, SizeResolver, Filter)))
            .Bind(items => Result.Try(() => Draw(items), exception => exception.Message));
    }

    private bool Filter(TagDrawingItem tag)
    {
        return classicDrawerSettings.ImageRectangle.Contains(tag.Rectangle);
    }

    private Size SizeResolver(TagDrawingItem tag)
    {
        var size = graphics.MeasureString(tag.Text, tag.Font).ToSize();
        size.Width += classicDrawerSettings.RectangleBorderSize * 2;
        size.Height += classicDrawerSettings.RectangleBorderSize * 2;
        return size;
    }

    private Font GetFont(CloudWord cloudWord)
    {
        var fontSize = Math.Clamp(classicDrawerSettings.MinimumTextFontSize + cloudWord.Occurrences * 3 - 3,
            classicDrawerSettings.MinimumTextFontSize, classicDrawerSettings.MaximumTextFontSize);
        return new(new FontFamily(classicDrawerSettings.TextFontFamily), fontSize,
            classicDrawerSettings.TextFontStyle);
    }

    private Result Draw(
        IEnumerable<TagDrawingItem> tagPropertyItems)
    {
        var rectangleBorderPen = new Pen(classicDrawerSettings.RectangleBorderBrush,
            classicDrawerSettings.RectangleBorderSize);
        var numbersFont = new Font(new FontFamily(classicDrawerSettings.NumbersFamily),
            classicDrawerSettings.NumbersSize);
        var withoutBordersLocationAddition = new Size(classicDrawerSettings.RectangleBorderSize,
            classicDrawerSettings.RectangleBorderSize);
        return Result.Success()
            .Tap(() => graphics.FillRectangle(classicDrawerSettings.BackgroundBrush,
                classicDrawerSettings.ImageRectangle))
            .Bind(() => Result.Combine(tagPropertyItems.Select((item, index) => DrawTag(
                withoutBordersLocationAddition,
                rectangleBorderPen,
                index + 1,
                numbersFont,
                item))));
    }

    private Result DrawTag(Size withoutBordersLocationAddition, Pen rectangleBorderPen, int index, Font numbersFont,
        TagDrawingItem item)
    {
        var textStartingPoint = item.Rectangle.Location + withoutBordersLocationAddition;

        return Result.Success()
            .TapIf(classicDrawerSettings.FillRectangles,
                () => graphics.FillRectangle(classicDrawerSettings.RectangleFillBrush, item.Rectangle))
            .TapIf(classicDrawerSettings.RectangleBorderSize != 0,
                () => graphics.DrawRectangle(rectangleBorderPen, item.Rectangle))
            .TapIf(classicDrawerSettings.WriteNumbers,
                () => graphics.DrawString(index.ToString(), numbersFont, classicDrawerSettings.NumbersBrush,
                    item.Rectangle))
            .Tap(() => graphics.DrawString(item.Text, item.Font, classicDrawerSettings.TextBrush, textStartingPoint));
    }
}