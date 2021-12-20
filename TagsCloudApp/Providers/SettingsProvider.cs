using System.Drawing;
using System.Drawing.Imaging;
using TagsCloudContainer.Filters;
using TagsCloudContainer.Infrastructure;
using TagsCloudContainer.Preprocessors;

namespace TagsCloudApp.Providers;

public static class SettingsProvider
{
    public static Settings GetSettings()
    {
        return new Settings
        {
            Palette = palette,
            Font = font,
            ImageSize = imageSize,
            Preprocessors = preprocessors,
            Filters = filters,
            Format = format
        };
    }

    private static readonly Palette palette = new Palette
    {
        Background = Color.Black,
        Primary = Color.Orange,
    };

    private static readonly Font font = new Font(FontFamily.GenericMonospace, 20);

    private static readonly Size imageSize = new Size(1000, 1000);

    private static readonly IPreprocessor[] preprocessors = new IPreprocessor[]
    {
        new TrimPreprocessor(),
        new MarkPreprocessor(),
        new LowercasePreprocessor()
    };

    private static readonly IFilter[] filters = new IFilter[]
    {
        new ExcludeFilter()
    };

    private static readonly ImageFormat format = ImageFormat.Png;
}