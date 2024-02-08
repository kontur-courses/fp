using System.Reflection;
using Autofac;
using MyStemWrapper;
using TagsCloudContainer;
using TagsCloudContainer.Settings;

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
        RegisterAssemblyTypes(builder, typeof(Tag).GetTypeInfo().Assembly);
        RegisterAssemblyTypes(builder, typeof(CommandLineParser).GetTypeInfo().Assembly);
        RegisterSettings(builder);


        builder.Register(context =>
            {
                var settings = context.Resolve<IAppSettings>();
                return new MyStem
                {
                    PathToMyStem = $@"{settings.ProjectDirectory}\mystem.exe",
                    Parameters = "-nli",
                };
            })
            .AsSelf();
    }

    private static void RegisterAssemblyTypes(ContainerBuilder builder, Assembly assembly)
    {
        builder.RegisterAssemblyTypes(assembly)
            .AsImplementedInterfaces();
    }

    private static void RegisterSettings(ContainerBuilder builder)
    {
        var assembly = typeof(ISettings).GetTypeInfo().Assembly;
        builder.RegisterAssemblyTypes(assembly)
            .Where(t => typeof(ISettings).IsAssignableFrom(t))
            .AsImplementedInterfaces()
            .SingleInstance();
    }
}