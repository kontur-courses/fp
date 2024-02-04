using Autofac;
using CommandLine;
using TagsCloudVisualization.CommandLine;

namespace TagsCloudVisualization;

public class Program
{
    public static void Main(string[] args)
    {
        var options = Parser.Default.ParseArguments<CommandLineOptions>(args).Value;

        var container = DiContainerBuilder.BuildContainer(options);
        var creator = container.Resolve<TagCloudCreator>();
        var result = creator.CreateAndSaveImage();
        if (!result.IsSuccess )
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(result.Error);
            Console.ResetColor();
        }
    }
}