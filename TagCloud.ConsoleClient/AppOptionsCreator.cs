using Microsoft.Extensions.Configuration;
using System.Drawing;

public static class AppOptionsCreator
{
    public static AppOptions CreateOptions(Options clOptions, IConfiguration defaultConfig)
    {
        var tagCloudOptions = CreateTagCloudOptions(clOptions, defaultConfig);
        var renderOptions = CreateRenderOptions(clOptions, defaultConfig);
        var wordExtractorOptions = CreateWordExtractionOptions(clOptions, defaultConfig);
        var serviceOptions = CreateServiceOptions(clOptions, defaultConfig);

        return new AppOptions()
        {
            DomainOptions = new DomainOptions()
            {
                WordExtractionOptions = wordExtractorOptions,
                RenderOptions = renderOptions,
                TagCloudOptions = tagCloudOptions,
                ServiceOptions = serviceOptions
            }
        };
    }

    private static TagCloudOptions CreateTagCloudOptions(Options options, IConfiguration defaultConfig)
    {
        return new TagCloudOptions
        {
            Center = ParsePoint(options.TagCloudCenter ?? defaultConfig["CenterPoint"]),
            MaxTagsCount = -1
        };
    }

    private static RenderOptions CreateRenderOptions(Options options, IConfiguration defaultConfig)
    {
        var fontSizeSpan = ParseFontSize(options.FontSize ?? defaultConfig["FontSettings:FontSize"]);

        return new RenderOptions()
        {
            ColorScheme = ColorScheme.Schemes[defaultConfig["ColorScheme"]],
            FontBase = CreateFontBase(options.FontFamily ?? defaultConfig["FontSettings:FontFamily"]),
            ImageSize = ParseSize(options.ImageSize ?? defaultConfig["ImageSize"]),
            MinFontSize = fontSizeSpan.min,
            MaxFontSize = fontSizeSpan.max,
        };
    }

    private static Font CreateFontBase(string fontFamily)
    {
        return new Font(fontFamily, 32, FontStyle.Regular, GraphicsUnit.Pixel);
    }

    private static WordExtractionOptions CreateWordExtractionOptions(Options clOptions, IConfiguration defaultConfig)
    {
        return new WordExtractionOptions() { MinWordLength = 4, PartsSpeech = PartSpeech.Noun | PartSpeech.Verb };
    }

    private static Size ParseSize(string str)
    {
        var coords = str.Split("x").Select(int.Parse).ToArray();
        return new Size(coords[0], coords[1]);
    }

    private static Point ParsePoint(string str)
    {
        var coords = str.Split("x").Select(int.Parse).ToArray();
        return new Point(coords[0], coords[1]);
    }

    private static (int min, int max) ParseFontSize(string str)
    {
        var sizes = str.Split(":").Select(int.Parse).ToArray();
        return (sizes[0], sizes[1]);
    }

    public static ServiceOptions CreateServiceOptions(Options clOptions, IConfiguration defaultConfig)
    {
        return new ServiceOptions()
        {
            FilterType = FilterType.MorphologicalFilter
        };
    }
}