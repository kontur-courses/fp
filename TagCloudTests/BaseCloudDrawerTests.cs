using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagCloud;
using TagCloud.Abstractions;

namespace TagCloudTests;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class BaseCloudDrawerTests
{
    [Test]
    public void Draw_ResultBitmap_ShouldFilledWithBackgroundColor_OnEmptyTagsCollection()
    {
        var backgroundColor = Color.FromArgb(255, 255, 255);
        var settings = DrawerSettings.Create(new Size(100, 100), backgroundColorName: "White").Value;
        var drawer = new BaseCloudDrawer(settings);

        var bitmap = drawer.Draw(Array.Empty<IDrawableTag>());

        AllPixels(bitmap).Should().OnlyContain(c => c == backgroundColor);
    }


    [Test]
    public void Draw_ShouldDrawSomeRectangles()
    {
        var textColor = Color.FromArgb(0, 0, 0);
        var settings = DrawerSettings.Create(new Size(100, 100),
            textColorName: "Black",
            backgroundColorName: "White").Value;
        var drawer = new BaseCloudDrawer(settings);
        var tags = new[] { new DrawableTag(new Tag("word", 0), 10, new Point(0, 0)) };

        var bitmap = drawer.Draw(tags);

        AllPixels(bitmap).Should().Contain(textColor);
    }

    [Test]
    public void DrawTagCloud_DifferentCloudsBitmaps_AreDifferent()
    {
        var settings = DrawerSettings.Create(new Size(100, 100),
            textColorName: "Black",
            backgroundColorName: "White").Value;
        var drawer = new BaseCloudDrawer(settings);
        var tags = Array.Empty<DrawableTag>();

        var bitmap1 = drawer.Draw(tags);
        tags = new[] { new DrawableTag(new Tag("word", 0), 10, new Point(0, 0)) };
        var bitmap2 = drawer.Draw(tags);

        AllPixels(bitmap1).Should().NotEqual(AllPixels(bitmap2));
    }

    private static IEnumerable<Color> AllPixels(Bitmap bitmap)
    {
        for (var x = 0; x < bitmap.Width; x++)
        for (var y = 0; y < bitmap.Height; y++)
            yield return bitmap.GetPixel(x, y);
    }
}