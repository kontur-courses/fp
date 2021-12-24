using System.Drawing;
using CommandLine;
using TagCloud;
using TagCloud.Configuration;
using TagCloud.PointGenerator;
using TagCloud.Templates.Colors;

namespace TagCloudApp.Apps;

public static class CommandLineConfigurationProvider
{
    public static Result<Configuration> GetConfiguration(IEnumerable<string> args)
    {
        return args.AsResult()
            .Then(Parser.Default.ParseArguments<ConsoleUiOptions>)
            .Then(a => a.Value)
            .Validate(v => v != null, "Invalid arguments")
            .Then(CheckArguments)
            .Then(CreateConfiguration);
    }


    private static Result<ConsoleUiOptions> CheckArguments(ConsoleUiOptions arguments)
    {
        if (!File.Exists(arguments.Filename))
        {
            return Result.Fail<ConsoleUiOptions>($"File {arguments.Filename} does not exist!");
        }

        if (arguments.Width <= 0 || arguments.Height <= 0)
            return Result.Fail<ConsoleUiOptions>($"Height and Width must be positive");

        if (arguments.CloudForm == null) return arguments;
        var cloudFrom = arguments.CloudForm.ToLower();
        if (cloudFrom != "spiral" && cloudFrom != "circle")
            return Result.Fail<ConsoleUiOptions>($"Cloud form may be spiral or circle, not {cloudFrom}");

        return arguments;
    }

    private static Result<Configuration> CreateConfiguration(ConsoleUiOptions arguments)
    {
        var configuration = new Configuration(arguments.Filename, arguments.Output)
        {
            ImageSize = new Size(arguments.Width, arguments.Height),
            BackgroundColor = arguments.BackgroundColor
        };
        if (arguments.Color != Color.Empty)
            configuration.ColorGenerator = new OneColorGenerator(arguments.Color);
        if (arguments.FontFamily != null)
            configuration.FontFamily = arguments.FontFamily;
        if (arguments.CloudForm != null)
            configuration.PointGenerator = arguments.CloudForm.ToLower() switch
            {
                "spiral" => Spiral.GetDefaultSpiral(),
                "circle" => Circle.GetDefault(),
                _ => throw new ArgumentException($"\"{arguments.CloudForm}\" is not correct generator name")
            };
        return configuration;
    }
}