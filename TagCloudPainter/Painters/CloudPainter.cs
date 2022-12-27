using System.Drawing;
using TagCloudPainter.Coloring;
using TagCloudPainter.Common;
using TagCloudPainter.ResultOf;

namespace TagCloudPainter.Painters;

public class CloudPainter : ICloudPainter
{
    private readonly IWordColoring _coloring;

    public CloudPainter(IImageSettingsProvider imageSettingsProvider, IWordColoring coloring)
    {
        ImageSettings = imageSettingsProvider.ImageSettings;
        _coloring = coloring;
    }

    private ImageSettings ImageSettings { get; }

    public Result<Bitmap> PaintTagCloud(IEnumerable<Tag> tags)
    {
        if (ImageSettings == null)
            return Result.Fail<Bitmap>("ImageSettings not setted");

        if (ImageSettings.Size.Width <= 0 || ImageSettings.Size.Height <= 0)
            return Result.Fail<Bitmap>("size is negative or zero");

        if (tags.Any(p => IsRectangleOutsideOfBorders(p.Rectangle)))
            return Result.Fail<Bitmap>("the tag cloud did not fit on the image of the given size");

        var btm = new Bitmap(ImageSettings.Size.Width, ImageSettings.Size.Height);
        var g = Graphics.FromImage(btm);
        g.Clear(ImageSettings.BackgroundColor);
        var format = new StringFormat
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };

        foreach (var tag in tags)
        {
            var font = tag.Count > 1
                ? new Font(ImageSettings.Font.FontFamily, ImageSettings.Font.Size + (tag.Count - 1), FontStyle.Bold,
                    GraphicsUnit.Point)
                : ImageSettings.Font;
            g.DrawString(tag.Value, font, new SolidBrush(_coloring.GetColor()), tag.Rectangle, format);
        }

        return btm;
    }

    private bool IsRectangleOutsideOfBorders(Rectangle rectangle)
    {
        return rectangle.X < 0
               || rectangle.Y < 0
               || rectangle.X + rectangle.Width >= ImageSettings.Size.Width
               || rectangle.Y + rectangle.Height >= ImageSettings.Size.Height;
    }
}