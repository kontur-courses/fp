using Autofac;
using TagCloud;

namespace ConsoleApp;

internal static class Program
{
    [STAThread]
    private static void Main(string[] args)
    {
        var options = ArgumentsParser.ParseArgs(args);

        if (args.Contains("--help"))
            return;

        if (!options.IsSuccess || options.Value is null)
        {
            Console.Error.WriteLine($"Arguments parsing: {options.Error}");
            return;
        }

        var properties = new ApplicationPropertiesSetuper(options)
            .Setup(new WordsParser());
        
        var container = DiContainerBuilder.Build(properties);
        
        var result = container.Resolve<TagCloudConstructor>().Construct();
        if (!result.IsSuccess)
        {
            Console.Error.WriteLine($"Cloud constructor:\n{result.Error}");
            return;
        }

        if (Path.GetExtension(options.Value.OutputPath) is not (".jpg" or ".jpeg" or ".png"))
            Console.Error.WriteLine("Unsupported image format in path");
        
        result.Value?.Save(options.Value.OutputPath);
        Console.WriteLine($"Tag cloud saved to file {options.Value.OutputPath}");
    }
}