using Autofac;
using TagCloud.Infrastructure.Pipeline;
using TagCloud.Infrastructure.Pipeline.Common;

namespace TagCloud;

public static class Program
{
    public static void Main(string[] args)
    {
        var appSettings = AppSettings.Parse(args);
        var builder = new ContainerBuilder();
        
        using var container = builder
            .ConfigureConsoleClient(appSettings)
            .Build();
        container
            .Resolve<IImagePipeline>()
            .Run(appSettings);
    }
}