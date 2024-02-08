using Autofac;
using CommandLine;
using TagsCloud.App;
using TagsCloud.ConsoleOptions;

namespace TagsCloud;

public class Program
{
    static void Main(string[] args)
    {
        var consoleArguments = CommandLine.Parser.Default.ParseArguments<ConsoleArguments>(args);
        var layouterOptions = OptionsParser.ParseOptions(consoleArguments.Value);

        if (!layouterOptions.IsSuccess)
        {
            Console.WriteLine(layouterOptions.Error);
            return;
        }
        
        var container = ContainerConfig.Configure(layouterOptions.Value);

        using var scope = container.BeginLifetimeScope();
        var app = scope.Resolve<IApp>();
        app.Run();
    }
    
}