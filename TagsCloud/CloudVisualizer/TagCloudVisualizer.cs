using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using TagsCloud.App.Settings;
using TagsCloud.CloudLayouter;
using TagsCloud.CloudPainter;
using TagsCloud.WordAnalyzer;

namespace TagsCloud.CloudVisualizer;

public class TagCloudVisualizer
{
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

    private bool IsRectangleOutOfBounds(Rectangle rectangle, Size imageSize)
    {
        return rectangle.Left < 0 
               || rectangle.Top < 0 
               || rectangle.Right > imageSize.Width 
               || imageSize.Height < rectangle.Bottom;
    }

   public Result<None> DrawTags(IEnumerable<WordInfo> words, Graphics graphics, ICloudLayouter cloudLayouter)
    {
        foreach (var word in words)
        {
            var tag = GetTag(word, cloudLayouter);
            var brush = new SolidBrush(tag.Color);
            if (IsRectangleOutOfBounds(tag.Rectangle, sizeImage))
            {
                return Result.Fail<None>("Облако тегов вышло за границы изображения.");
            }
            graphics.DrawString(tag.Word, tag.Font, brush, tag.Rectangle.Location);
        }

        return Result.Ok();
    }
}