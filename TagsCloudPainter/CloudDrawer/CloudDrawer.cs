using ResultLibrary;
using System.Drawing;
using System.Drawing.Drawing2D;
using TagsCloudPainter.Settings.Cloud;
using TagsCloudPainter.Settings.Tag;

namespace TagsCloudPainter.Drawer;

public class CloudDrawer : ICloudDrawer
{
    private readonly ICloudSettings cloudSettings;
    private readonly ITagSettings tagSettings;

    public CloudDrawer(ITagSettings tagSettings, ICloudSettings cloudSettings)
    {
        this.tagSettings = tagSettings ?? throw new ArgumentNullException(nameof(tagSettings));
        this.cloudSettings = cloudSettings ?? throw new ArgumentNullException(nameof(cloudSettings));
    }

    public Result<Bitmap> DrawCloud(TagsCloud cloud, int imageWidth, int imageHeight)
    {
        if (cloud.Tags.Count == 0)
            return Result.Fail<Bitmap>("rectangles are empty");
        if (imageWidth <= 0 || imageHeight <= 0)
            return Result.Fail<Bitmap>("either width or height of image is not positive");

        var drawingScale = CalculateObjectDrawingScale(cloud.GetWidth(), cloud.GetHeight(), imageWidth, imageHeight);
        if (!drawingScale.IsSuccess)
            return Result.Fail<Bitmap>(drawingScale.Error);

        var bitmap = new Bitmap(imageWidth, imageHeight);
        using var graphics = Graphics.FromImage(bitmap);
        using var pen = new Pen(tagSettings.TagColor);
        {
            graphics.TranslateTransform(-cloud.Center.X, -cloud.Center.Y);
            graphics.ScaleTransform(drawingScale.GetValueOrThrow(), drawingScale.GetValueOrThrow(), MatrixOrder.Append);
            graphics.TranslateTransform(cloud.Center.X, cloud.Center.Y, MatrixOrder.Append);
            graphics.Clear(cloudSettings.BackgroundColor);
            foreach (var tag in cloud.Tags)
            {
                var font = new Font(tagSettings.TagFont, tag.Item1.FontSize);
                graphics.DrawString(tag.Item1.Value, font, pen.Brush, tag.Item2.Location);
            }
        };

        return bitmap;
    }

    public static Result<float> CalculateObjectDrawingScale(float width, float height, float imageWidth, float imageHeight)
    {
        if (width <= 0 || height <= 0)
            return Result.Fail<float>("Scale can't be calculated of object with not positive width or height");

        var scale = 1f;
        var scaleAccuracy = 0.05f;
        var widthScale = scale;
        var heightScale = scale;
        if (width * scale > imageWidth)
            widthScale = imageWidth / width - scaleAccuracy;
        if (height * scale > imageHeight)
            heightScale = imageHeight / height - scaleAccuracy;
        return Math.Min(widthScale, heightScale);
    }
}