using System.Drawing;
using TagsCloudVisualization.CloudLayouter;
using TagsCloudVisualization.CloudLayouter.PointGenerator;
using TagsCloudVisualization.ColorGenerator;
using TagsCloudVisualization.FontSettings;
using TagsCloudVisualization.ImageSavers;
using TagsCloudVisualization.ImageSettings;
using TagsCloudVisualization.TextProviders;

namespace TagsCloudVisualization.CLI.Extensions;

public static class OptionsExtensions
{
    public static Result<TagsCloudVisualizationSettings> GetVisualizationSettings(this Options options)
    {
        return Result.Of(() =>
            new TagsCloudVisualizationSettings()
            {
                Filepath = Path.Combine(Options.DefaultDirectory, options.Filepath),
                OutputDirectory = options.OutputDirectory ?? Options.DefaultOutputDirectory,
                BackgroundColor = options.BackgroundColor,
                Width = options.Width,
                Height = options.Height,
                FontSize = options.FontSize,
                FontFamily = options.FontFamily,
                ColorAlgorithm = options.ColorAlgorithm,
                ImageFileExtension = options.ImageFileExtension,
                LayoterAlgorithm = options.LayoterAlgorithm,
                TagCount = options.TagCount
            });
    }
}