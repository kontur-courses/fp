using Microsoft.Extensions.DependencyInjection;
using TagsCloudResult;
using TagsCloudResult.Image;
using TagsCloudResult.TagCloud;
using TagsCloudResult.UI;
using TagsCloudResult.Utility;

public class DIContainer
{
    public static ServiceProvider ContainerInit(ApplicationArguments args)
    {
        ServiceCollection services = [];

        services.AddSingleton<IUI, CLI>();

        services.AddSingleton(args);

        services.AddTransient<ICircularCloudLayouter, CircularCloudLayouter>();
        services.AddTransient<ImageGenerator>();
        services.AddTransient<TagCloudVisualizer>();

        services.AddSingleton<ITextHandler, FileTextHandler>();
        services.AddSingleton<WordHandler>();
        services.AddTransient<WordDataSet>();

        services.AddSingleton<Application>();

        return services.BuildServiceProvider();
    }
}