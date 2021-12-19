using System.Drawing.Imaging;

namespace TagsCloudContainer.Infrastructure.Providers;

public static class ImageSettingsProvider
{
    public static Color[] GetColors()
    {
        return new[]
        {
            Color.Red,
            Color.Orange,
            Color.Yellow,
            Color.Green,
            Color.Blue,
            Color.DarkBlue,
            Color.Violet,
            Color.Black,
            Color.White,
        };
    }

    public static FontFamily[] GetFonts()
    {
        return new[]
        {
            FontFamily.GenericMonospace,
            FontFamily.GenericSansSerif,
            FontFamily.GenericSerif,
        };
    }

    public static FontStyle[] GetStyles()
    {
        return new[]
        {
            FontStyle.Bold,
            FontStyle.Italic,
            FontStyle.Regular,
            FontStyle.Strikeout,
            FontStyle.Underline,
        };
    }

    public static ImageFormat[] GetFormats()
    {
        return new[]
        {
            ImageFormat.Bmp,
            ImageFormat.Png,
            ImageFormat.Jpeg,
        };
    }
}
