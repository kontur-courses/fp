using Autofac;
using TagCloud;
using TagCloudApp.Apps;
using TagCloudApp.Configurations;
using Configuration = TagCloudApp.Configurations.Configuration;


namespace TagCloudApp;

public static class Program
{
    public static void Main(string[] args)
    {
        var config = CommandLineConfigurationProvider.GetConfiguration(args);
        config
            .Validate(c => c != null)
            .Then(BuildContainer)
            .Then(c => c.Resolve<IApp>())
            .Then(a => a.Run())
            .OnFail(HandleError);
    }

    private static void HandleError(string message)
    {
        Console.WriteLine($"[ERROR] {message}");
    }

    private static IContainer BuildContainer(Configuration configuration)
    {
        var builder = new ContainerBuilder().GetDefaultBuild(configuration);
        builder.RegisterType<ConsoleApp>().As<IApp>();
        return builder.Build();
    }
}