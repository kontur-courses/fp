using Autofac;
using CommandLine;
using Results;
using TagsCloudVisualization.CommandLine;

namespace TagsCloudVisualization;

public class Program
{
    public static void Main(string[] args)
    {
        var options = Parser.Default.ParseArguments<CommandLineOptions>(args).Value;
        var optionsCheck = SettingsChecker.CheckSettings(options);
        if (!optionsCheck.IsSuccess)
        {
            WriteError(optionsCheck);
            return;
        }

        var container = DiContainerBuilder.BuildContainer(options);
        var creator = container.Resolve<TagCloudCreator>();
        var result = creator.CreateAndSaveImage();
        if (!result.IsSuccess)
            WriteError(result);
    }

    private static void WriteError(Result<None> result)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(result.Error);
        Console.ResetColor();
    }
}