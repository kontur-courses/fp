using Mono.Options;
using System.Drawing.Drawing2D;
using TagsCloudContainer.Abstractions;

namespace TagsCloudContainer.Defaults.SettingsProviders;

public class VisualizerSettings : ICliSettingsProvider
{
    private const int defaultWidth = 800;
    private const int defaultHeight = 400;
    private const SmoothingMode defaultSmoothingMode = SmoothingMode.AntiAlias;
    private const int defaultWordLimit = 0;

    public int Width { get; private set; } = defaultWidth;
    public int Height { get; private set; } = defaultHeight;
    public SmoothingMode SmoothingMode { get; private set; } = defaultSmoothingMode;
    public int WordLimit { get; private set; } = defaultWordLimit;

    public OptionSet GetCliOptions()
    {
        var options = new OptionSet()
        {
            { "width=", $"Set width of resulting bitmap. Defaults to {defaultWidth}", (int v) => Width = v },
            { "height=", $"Set height of resulting bitmap. Defaults to {defaultHeight}", (int v) => Height = v },
            { "word-limit=", $"Set word limit to use. 0 means no limit Defaults to {defaultWordLimit}", (int v) => WordLimit = v },
            { "smoothing-mode=",$"Set smoothing mode for resulting bitmap. Defaults to {defaultSmoothingMode}", (SmoothingMode v) => SmoothingMode = v },
        };

        return options;
    }
}
