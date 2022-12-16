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

        var container = DiContainerBuilder.Build();
        
        new ApplicationPropertiesSetuper(options)
            .Setup(
                container.Resolve<ApplicationProperties>(),
                container.Resolve<IWordsParser>());

        var result = container.Resolve<TagCloudConstructor>().Construct();
        if (!result.IsSuccess)
        {
            Console.Error.WriteLine($"Cloud constructor:\n{result.Error}");
            return;
        }

        result.Value?.Save(options.Value.OutputPath);
        Console.WriteLine($"Tag cloud saved to file {options.Value.OutputPath}");
    }
}