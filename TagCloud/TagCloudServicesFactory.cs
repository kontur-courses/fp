using System.Drawing;
using Microsoft.Extensions.DependencyInjection;
using TagCloud.CloudDrawers;
using TagCloud.ColorSelectors;
using TagCloud.TextHandlers;
using TagCloud.WordFilters;
using ColorConverter = TagCloud.Extensions.ColorConverter;

namespace TagCloud;

public static class TagCloudServicesFactory
{
    public static ServiceCollection ConfigureService(Options options)
    {
        var services = new ServiceCollection();
        
        services.AddSingleton<TagCloudGenerator>();
                
        services.AddSingleton<ICloudShaper>(provider => SpiralCloudShaper.Create(new Point(0, 0), coefficient: options.Density));
        services.AddSingleton<CircularCloudLayouter>();
        
        services.AddScoped<Font>(provider => new Font(new FontFamily(options.Font), options.FontSize));
        services.AddSingleton<TextMeasurer>();
        services.AddSingleton<ICloudDrawer>(provider => TagCloudDrawer.Create(
                options.DestinationPath, 
                options.Name, 
                provider.GetService<Font>(),
                provider.GetService<IColorSelector>()
            )
        );
        
        
        if (options.ColorScheme == "random")
            services.AddSingleton<IColorSelector, RandomColorSelector>();
        else if (ColorConverter.TryConvert(options.ColorScheme, out var color))
            services.AddSingleton<IColorSelector>(provider => new ConstantColorSelector(color));
        else
            services.AddSingleton<IColorSelector>(provider => new ConstantColorSelector(Color.Black));

        services.AddSingleton<IWordFilter, MyStemWordFilter>();

        var stream = File.Open(options.SourcePath, FileMode.Open);
        services.AddSingleton<ITextHandler>(provider => 
            new FileTextHandler(
                stream,
                filter: provider.GetService<IWordFilter>()
            )
        );
        return services;
    }

    public static Result<T> ConfigureServiceAndGet<T>(Options option)
    {
        var result = Result.Of(() => ConfigureService(option));
        if (!result.IsSuccess) return Result.Fail<T>(result.Error);
        using var serviceProvider = result.Value.BuildServiceProvider();
        return Result.FailIfNull(() => serviceProvider.GetService<T>());
    }
}