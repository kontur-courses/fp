using System.Drawing;
using DocumentFormat.OpenXml.Wordprocessing;
using TagCloud.Abstractions;
using Font = System.Drawing.Font;
using FontFamily = System.Drawing.FontFamily;

namespace TagCloud;

public class BaseCloudDrawer : ICloudDrawer
{
    private Bitmap bitmap;
    public DrawerSettings Settings { get; }
    public BaseCloudDrawer(DrawerSettings settings)
    {
        Settings = settings;
        bitmap = new Bitmap(Settings.ImageSize.Width, Settings.ImageSize.Height);
        Graphics = Graphics.FromImage(bitmap);
    }

    public Graphics Graphics { get; private set; }

    public Bitmap Draw(IEnumerable<IDrawableTag> tags)
    {
        Graphics.Clear(Settings.BackgroundColor);
        foreach (var tag in tags)
        {
            using var font = new Font(Settings.FontFamily, tag.FontSize);
            using var brush = new SolidBrush(Settings.TextColor);
            Graphics.DrawString(tag.Tag.Text, font, brush, tag.Location);
        }

        var result = bitmap;
        bitmap = new Bitmap(Settings.ImageSize.Width, Settings.ImageSize.Height);
        Graphics = Graphics.FromImage(bitmap);
        return result;
    }
}