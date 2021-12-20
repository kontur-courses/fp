using Mono.Options;
using ResultExtensions;
using ResultOf;
using System.Drawing;
using TagsCloudContainer.Abstractions;

namespace TagsCloudContainer.Defaults.SettingsProviders;

public class LayouterSettingsProvider : ICliSettingsProvider
{
    private static readonly Point defaultCenter = new(300, 100);

    public Point Center { get; private set; } = defaultCenter;

    public Result State { get; private set; } = Result.Ok();
    public OptionSet GetCliOptions()
    {
        var options = new OptionSet()
        {
            { "center=", $"Coordinates of center for CircularLayouter, separated by ','. Defaults to {defaultCenter}", v => State = Parse(v).Then(c => Center = c) }
        };

        return options;
    }

    private static Result<Point> Parse(string v)
    {
        var coords = v.Split(',');
        if (coords.Length != 2)
            return Result.Fail<Point>($"String {v} was in incorrect format, should be two ints separated by ','");
        return new Point(int.Parse(coords[0]), int.Parse(coords[1]));
    }
}