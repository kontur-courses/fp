using System.Drawing;
using System.Drawing.Imaging;
using ResultOf;
using TagCloud.Common.Extensions;
using TagCloud.Common.Options;

namespace TagCloud.Common.Drawing;

public class CloudDrawer : ICloudDrawer
{
    public DrawingOptions CloudDrawingOptions { get; }
    private Point ImageShift { get; set; }

    public CloudDrawer(DrawingOptions cloudDrawingOptions)
    {
        CloudDrawingOptions = cloudDrawingOptions;
    }

    public Result<Bitmap> DrawCloud(IEnumerable<Tag> tags)
    {
        var newbmp = new Bitmap(CloudDrawingOptions.ImageSize.Width, CloudDrawingOptions.ImageSize.Height);
        var center = tags.First().Bounds.FindCenter();
        ImageShift = new Point(newbmp.Width / 2 - center.X, newbmp.Height / 2 - center.Y);
        if (!FitsImageSize(tags))
        {
            return Result.Fail<Bitmap>("Tags don't fit the cloud image size. Try to use larger size of image");
        }

        using (var graphics = Graphics.FromImage(newbmp))
        {
            graphics.Clear(CloudDrawingOptions.BackgroundColor);
            graphics.TranslateTransform(ImageShift.X, ImageShift.Y);
            foreach (var tag in tags)
            {
                var pen = new Pen(CloudDrawingOptions.TextColor);
                graphics.DrawString(tag.Word, tag.Font, new SolidBrush(pen.Color), tag.Bounds);
            }
        }

        return newbmp.AsResult();
    }

    private bool FitsImageSize(IEnumerable<Tag> tags)
    {
        var imageBounds = new Rectangle(new Point(-ImageShift.X, -ImageShift.Y), CloudDrawingOptions.ImageSize);
        return tags.All(tag => imageBounds.Contains(tag.Bounds));
    }
}