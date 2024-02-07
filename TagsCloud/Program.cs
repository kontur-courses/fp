using Autofac;
using CommandLine;
using TagsCloud.App;
using TagsCloud.ConsoleCommands;

namespace TagsCloud;

public class Program
{
    static void Main(string[] args)
    {
        var options = CommandLine.Parser.Default.ParseArguments<Options>(args);
        Console.WriteLine(options.Errors.Any());
        var container = ContainerConfig.Configure(options.Value);

        using var scope = container.BeginLifetimeScope();
        var app = scope.Resolve<IApp>();
        app.Run();
    }
}