using Autofac;
using TagsCloudContainer;

namespace ConsoleApp;

public class Program
{
    public static void Main()
    {
        var builder = new ContainerBuilder();
        ConfigureService(builder);
        var container = builder.Build();

        using var scope = container.BeginLifetimeScope();
        var commandLineReader = scope.Resolve<ICommandLineParser>();
        commandLineReader.ParseFromConsole();
    }

    public static void ConfigureService(ContainerBuilder builder)
    {
        builder.RegisterModule<TagsCloudContainerModule>();
        builder.RegisterModule<ConsoleAppModule>();
    }
}