using System.Drawing;
using System.Windows.Forms;
using TagsCloud.App.Settings;
using TagsCloud.CloudLayouter;
using TagsCloud.CloudPainter;
using TagsCloud.WordAnalyzer;

namespace TagsCloud.CloudVisualizer;

public class TagCloudVisualizer
{
    public const int Border = 35;
    private readonly string Link = "https://github.com/lepeap/DeepMorphy/blob/master/README.md";
    private readonly Size sizeImage;
    private readonly TagSettings tagSettings;

    public TagCloudVisualizer(TagSettings tagSettings, Size imageSize)
    {
        sizeImage = imageSize;
        this.tagSettings = tagSettings;
    }

    private Tag GetTag(WordInfo wordInfo, ICloudLayouter cloudLayouter)
    {
        var font = new Font(tagSettings.FontFamily, wordInfo.Count * tagSettings.Size);
        var textSize = TextRenderer.MeasureText(wordInfo.Word, font);
        var textRectangle = cloudLayouter.PutNextRectangle(new Size(textSize.Width, textSize.Height));
        return new Tag(font, wordInfo.Word, textRectangle.GetValueOrThrow(), tagSettings.Color);
    }

    private Rectangle MakeNeedImageSize(Rectangle rectangle, Rectangle imageRectangle)
    {
        var leftmost = Math.Min(imageRectangle.Left, rectangle.Left);
        var rightmost = Math.Max(imageRectangle.Right, rectangle.Right);
        var topmost = Math.Min(imageRectangle.Top, rectangle.Top);
        var bottommost = Math.Max(imageRectangle.Bottom, rectangle.Bottom);
        return new Rectangle(leftmost, topmost, rightmost - leftmost, bottommost - topmost);
    }

    private bool IsRectangleOutOfBounds(Rectangle expectedImage)
    {
        return sizeImage.Height < expectedImage.Height || sizeImage.Width < expectedImage.Width;
    }

    public Result<None> DrawTags(IEnumerable<WordInfo> words, Graphics graphics, ICloudLayouter cloudLayouter)
    {
        var imageRectangle = new Rectangle(new Point(0, 0), sizeImage);
        foreach (var word in words)
        {
            var tag = GetTag(word, cloudLayouter);
            var brush = new SolidBrush(tag.Color);
            imageRectangle = MakeNeedImageSize(tag.Rectangle, imageRectangle);
            graphics.DrawString(tag.Word, tag.Font, brush, tag.Rectangle.Location);
        }

        return IsRectangleOutOfBounds(imageRectangle)
            ? Result.Fail<None>(
                $"Облако тегов вышло за границы изображения. Поставь размер {imageRectangle.Width + Border * 2}x{imageRectangle.Height + Border * 2}")
            : Result.Ok();
    }
}