using Autofac;
using CommandLine;
using TagsCloudContainer.App;
using TagsCloudContainer.WordProcessing;

namespace TagsCloudContainer;

internal static class Program
{
    public static void Main(string[] args)
    {
        var options = Parser.Default.ParseArguments<CommandLineOptions>(args).Value;
        var container = Container.SetDiBuilder(options);
        var app = container.Resolve<IApp>();
        app.Run().OnFail(Console.WriteLine);
    }
}
