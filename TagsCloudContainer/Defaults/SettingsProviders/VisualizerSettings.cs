using Mono.Options;
using ResultExtensions;
using ResultOf;
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

    public Result State { get; private set; } = Result.Ok();

    public OptionSet GetCliOptions()
    {
        var options = new OptionSet()
        {
            { "width=", $"Set width of resulting bitmap. Defaults to {defaultWidth}", v => State = ParseInt(v).Then(w => Width = w) },
            { "height=", $"Set height of resulting bitmap. Defaults to {defaultHeight}", v => State = ParseInt(v).Then(h => Height = h) },
            { "word-limit=", $"Set word limit to use. 0 means no limit Defaults to {defaultWordLimit}", v => State = ParseInt(v).Then(wl => WordLimit = wl) },
            { "smoothing-mode=",$"Set smoothing mode for resulting bitmap. Defaults to {defaultSmoothingMode}", v => State = ParseSmoothingMode(v).Then(sm => SmoothingMode = sm) },
        };

        return options;
    }

    private static Result<int> ParseInt(string v)
    {
        return int.TryParse(v, out var r) ? r : Result.Fail<int>($"Could not parse {v} as {nameof(Int32)}");
    }

    private static Result<SmoothingMode> ParseSmoothingMode(string v)
    {
        return Enum.TryParse<SmoothingMode>(v, out var r) ? r : Result.Fail<SmoothingMode>($"Could not parse {v} as {nameof(System.Drawing.Drawing2D.SmoothingMode)}");
    }
}
