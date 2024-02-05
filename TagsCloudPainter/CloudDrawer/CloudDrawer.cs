using System.Drawing;
using System.Drawing.Drawing2D;
using ResultLibrary;
using TagsCloudPainter.Extensions;
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

        var pen = Result.Of(() => new Pen(tagSettings.TagColor));
        var bitmap = Result.Of(() => new Bitmap(imageWidth, imageHeight));
        var graphics = bitmap.Then(Graphics.FromImage);
        var scalingCloudResult = graphics.Then(graphics => ScaleCloud(graphics, cloud, imageWidth, imageHeight));
        var drawingResult = graphics.Then(graphics => pen.Then(pen => 
        DrawTags(graphics, cloud, tagSettings.TagFont, cloudSettings.BackgroundColor, pen)));

        var failedResult = ResultExtension.GetFirstFailedResult(pen, graphics, scalingCloudResult, drawingResult);
        if (!failedResult.IsSuccess)
            return Result.Fail<Bitmap>(failedResult.Error);

        return bitmap;
    }

    private static Result<None> ScaleCloud(Graphics graphics, TagsCloud cloud, int width, int height)
    {
        var drawingScale = CalculateObjectDrawingScale(cloud.GetWidth(), cloud.GetHeight(), width, height);
        if (!drawingScale.IsSuccess)
            return Result.Fail<None>(drawingScale.Error);

        var scalingResult = new Result<None>();
        scalingResult = scalingResult.Then(() => graphics.TranslateTransform(-cloud.Center.X, -cloud.Center.Y));
        scalingResult = scalingResult.Then(() =>
            graphics.ScaleTransform(drawingScale.GetValueOrThrow(), drawingScale.GetValueOrThrow(),
                MatrixOrder.Append));
        scalingResult = scalingResult.Then(() =>
            graphics.TranslateTransform(cloud.Center.X, cloud.Center.Y, MatrixOrder.Append));

        return scalingResult;
    }

    private static Result<float> CalculateObjectDrawingScale(float width, float height, float imageWidth,
        float imageHeight)
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

    private static Result<None> DrawTags(Graphics graphics, TagsCloud cloud, FontFamily tagFont, Color backgroundColor,
        Pen pen)
    {
        var drawingResult = new Result<None>();
        drawingResult = drawingResult.Then(() => graphics.Clear(backgroundColor));
        foreach (var tag in cloud.Tags)
        {
            var font = new Font(tagFont, tag.Item1.FontSize);
            drawingResult = drawingResult.Then(() => graphics.DrawString(tag.Item1.Value, font, pen.Brush, tag.Item2.Location));
        }

        return drawingResult;
    }
}