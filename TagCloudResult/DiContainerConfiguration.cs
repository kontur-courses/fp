using Microsoft.Extensions.DependencyInjection;
using TagCloudResult.Clients;
using TagCloudResult.Files;
using TagCloudResult.Savers;
using TagCloudResult.Words;

namespace TagCloudResult;

public static class DiContainerConfiguration
{
    internal static ServiceProvider Build()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddSingleton<IFileReader, TxtFileReader>();
        services.AddSingleton<IFileReader, DocFileReader>();
        services.AddSingleton<FileReader>();
        services.AddSingleton<Random>();
        services.AddSingleton<CloudDrawer>();
        services.AddSingleton<CloudLayouter>();
        services.AddSingleton<TextFormatter>();
        services.AddSingleton<IBitmapSaver, HardDriveSaver>();
        services.AddSingleton<Client>();
        return services.BuildServiceProvider();
    }
}