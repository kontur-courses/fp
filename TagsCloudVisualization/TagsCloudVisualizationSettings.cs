using TagsCloudVisualization.CloudLayouter;
using TagsCloudVisualization.ColorGenerator;
using TagsCloudVisualization.FontSettings;
using TagsCloudVisualization.ImageSavers;
using TagsCloudVisualization.ImageSettings;
using TagsCloudVisualization.TextProviders;

namespace TagsCloudVisualization;

public class TagsCloudVisualizationSettings
{
    public string Filepath { get; set; } = string.Empty;
    public string OutputDirectory { get; set; } = string.Empty;

    public int Height { get; set; }

    public int Width { get; set; }

    public string BackgroundColor { get; set; } = string.Empty;

    public string FontFamily { get; set; } = string.Empty;

    public int FontSize { get; set; }

    public string LayoterAlgorithm { get; set; } = string.Empty;

    public string ColorAlgorithm { get; set; } = string.Empty;

    public string ImageFileExtension { get; set; } = string.Empty;
    public IReadOnlyCollection<string> BoringWords { get; set; } = Array.Empty<string>();
    public int TagCount { get; set; }
}