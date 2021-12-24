using System.Drawing;
using CommandLine;
using TagCloud;
using TagCloud.PointGenerator;
using TagCloud.Templates.Colors;

namespace TagCloudApp.Configurations;

public static class CommandLineConfigurationProvider
{
    public static Result<Configuration> GetConfiguration(IEnumerable<string> args)
    {
        return args.AsResult()
            .Then(Parser.Default.ParseArguments<ConsoleUiOptions>)
            .Then(a => a.Value)
            .Validate(v => v != null)
            .Then(CheckArguments)
            .Then(CreateConfiguration);
    }


    private static ConsoleUiOptions CheckArguments(ConsoleUiOptions arguments)
    {
        if (!File.Exists(arguments.Filename))
        {
            throw new FileNotFoundException($"File {arguments.Filename} does not exist!");
        }

        return arguments;
    }

    private static Configuration CreateConfiguration(ConsoleUiOptions arguments)
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