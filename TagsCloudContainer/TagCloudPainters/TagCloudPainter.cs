using TagsCloudContainer.Infrastructure;
using TagsCloudContainer.Tags;

namespace TagsCloudContainer.TagCloudPainters;

public class TagCloudPainter : ITagCloudPainter
{
    private const int AddedImageSize = 300;
    private readonly Settings settings;

    public TagCloudPainter(Settings settings)
    {
        this.settings = settings;
    }

    public Bitmap Paint(IEnumerable<CloudTag> cloudTags)
    {
        var neededSize = CalculateCoverageSize(cloudTags
                             .Select(tag => tag.Rectangle).ToArray())
                         + new Size(AddedImageSize, AddedImageSize);
        var imageOffset = neededSize / 2 - new Size(settings.Center);
        var scaleX = settings.ImageSize.Width / (float)neededSize.Width;
        var scaleY = settings.ImageSize.Height / (float)neededSize.Height;
        return DrawTags(scaleX, scaleY, cloudTags, imageOffset);
    }

    private Bitmap DrawTags(float scaleX, float scaleY,
        IEnumerable<CloudTag> cloudTags, Size imageOffset)
    {
        var bm = new Bitmap(settings.ImageSize.Height, settings.ImageSize.Width);
        var graphics = Graphics.FromImage(bm);
        graphics.ScaleTransform(scaleX, scaleY);
        graphics.Clear(settings.Palette.Background);

        foreach (var tag in cloudTags)
        {
            tag.Rectangle.Offset(new Point(imageOffset));
            graphics.DrawString(tag.Text, tag.Label.Font,
                new SolidBrush(tag.Color), tag.Rectangle);
        }

        return bm;
    }

    private static Size CalculateCoverageSize(Rectangle[] rectangles)
    {
        var maxX = rectangles.Max(x => x.X + x.Size.Width);
        var minX = rectangles.Min(x => x.X);
        var width = maxX - minX;

        var maxY = rectangles.Max(x => x.Y);
        var minY = rectangles.Min(x => x.Y - x.Size.Height);
        var height = maxY - minY;

        return new Size(width, height);
    }
}