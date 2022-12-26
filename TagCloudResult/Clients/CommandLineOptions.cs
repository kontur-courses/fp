using System.Drawing;
using CommandLine;
using ResultOfTask;

namespace TagCloudResult.Clients;

public class CommandLineOptions
{
    [Option('i', "input", Required = true, HelpText = "Input file with words")]
    public string InputFile { get; set; }

    [Option('o', "output", Required = true, HelpText = "Output image")]
    public IEnumerable<string> OutputFiles { get; set; }

    [Option('c', "colors", Required = false, Default = null, HelpText = "Colors to be used in image")]
    public IEnumerable<Color> Colors { get; set; }

    [Option('h', "height", Required = false, Default = 1080, HelpText = "Height of image")]
    public int Height { get; set; }

    [Option('w', "width", Required = false, Default = 1920, HelpText = "Width of image")]
    public int Width { get; set; }

    [Option("fontFamily", Required = false, Default = "Times New Roman", HelpText = "Font name")]
    public string FontFamily { get; set; }

    [Option("fontSize", Required = false, Default = 50, HelpText = "Font size")]
    public float FontSize { get; set; }

    [Option('a', "algorithm", Required = false, Default = "Spiral", HelpText = "Algorithm for generating image")]
    public string Curve { get; set; }

    public static Result<ParserResult<CommandLineOptions>> Parse(string[] args)
    {
        var parsedArguments = Parser.Default.ParseArguments<CommandLineOptions>(args);
        return parsedArguments.Errors.Any()
            ? Result.Fail<ParserResult<CommandLineOptions>>("Couldn't parse arguments")
            : Result.Ok(parsedArguments);
    }
}