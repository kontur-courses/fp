using System.Drawing;
using Microsoft.Extensions.DependencyInjection;
using TagsCloudVisualization.CloudLayouter;
using TagsCloudVisualization.CloudLayouter.PointGenerator;
using TagsCloudVisualization.ColorGenerator;
using TagsCloudVisualization.Drawer;
using TagsCloudVisualization.FontSettings;
using TagsCloudVisualization.ImageSavers;
using TagsCloudVisualization.ImageSettings;
using TagsCloudVisualization.Preprocessors;
using TagsCloudVisualization.TagFactory;
using TagsCloudVisualization.TextProviders;
using TagsCloudVisualization.ToTagConverter;

namespace TagsCloudVisualization.CLI;

public static class Bootstrapper
{
    public static IServiceCollection AddTagCloudVisualization(this IServiceCollection services,
        TagsCloudVisualizationSettings settings)
    {
        services.AddScoped<Visualizer>();
        services.AddScoped<ITagFactory, TagFactory.TagFactory>();
        services.AddScoped(_ => GetCloudLayouterAlgorithm(settings.LayoterAlgorithm));
        services.AddScoped<IToTagConverter, WordToTagConverter>();
        services.AddScoped(_ => GetColorGenerator(settings.ColorAlgorithm));
        services.AddScoped(_ => GetTextProvider(settings.Filepath));
        services.AddScoped<IDrawer, ImageDrawer>();
        services.AddScoped(_ => GetImageSaver(settings.ImageFileExtension));
        services.AddScoped<IFontSettingsProvider>(_ =>
            new FontSettingsProvider(settings.FontSize, settings.FontFamily));
        services.AddScoped<IImageSettingsProvider>(_ => new ImageSettingsProvider(
            Color.FromName(settings.BackgroundColor),
            settings.Width,
            settings.Height));
        services.AddPreprocessors(settings);
        return services;
    }

    private static ITextProvider GetTextProvider(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentException("File path not set");
        }

        var extension = Path.GetExtension(path).TrimStart('.');
        return extension switch
        {
            "txt" => new TxtTextProvider(path),
            "doc" => new DocTextProvider(path),
            _ => throw new Exception($"Not found text reader for *.{extension}")
        };
    }

    private static IColorGenerator GetColorGenerator(string name)
    {
        return name switch
        {
            "rainbow" => new RainbowColorGenerator(new Random()),
            "random" => new RandomColorGenerator(new Random()),
            _ => throw new ArgumentException("Such color generator not supported")
        };
    }

    private static AbstractImageSaver GetImageSaver(string extension)
    {
        return extension switch
        {
            "jpeg" => new JpegImageSaver(),
            "png" => new PngImageSaver(),
            _ => throw new ArgumentException("Such image saver not supported")
        };
    }

    private static ICloudLayouter GetCloudLayouterAlgorithm(string name)
    {
        var point = Point.Empty;
        var algorithm = name switch
        {
            "circular" => new CircularCloudLayouter(new SpiralPointGenerator(point)),
            "random" => new CircularCloudLayouter(new RandomPointGenerator(new Random())),
            _ => throw new ArgumentException("Such layouter algorithm not supported")
        };
        return algorithm;
    }

    private static void AddPreprocessors(this IServiceCollection services, TagsCloudVisualizationSettings settings)
    {
        var preprocessors = new List<IPreprocessor>
        {
            new LowerCasePreprocessor(),
            new BoringPreprocessor(settings.BoringWords.ToList())
        };

        services.AddSingleton<IPreprocessor>(_ => new MultiPreprocessor(preprocessors));
    }
}