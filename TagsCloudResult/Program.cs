﻿using Microsoft.Extensions.DependencyInjection;
using TagsCloudResult.Image;
using TagsCloudResult.TagCloud;
using TagsCloudResult.UI;
using TagsCloudResult.Utility;

namespace TagsCloudResult;

public static class Program
{
    /*
    -i="/Users/draginsky/Rider/fp/TagsCloudResult/src/words.txt"
    -o="/Users/draginsky/Rider/fp/TagsCloudResult/out/res"
    --fontpath="/Users/draginsky/Rider/fp/TagsCloudResult/src/JosefinSans-Regular.ttf"
     */
    public static void Main(string[] args)
    {
        using var container = ContainerInit(args);

        var app = container.GetService<Application>()!;

        app.Run(container.GetService<ApplicationArguments>()!);
    }

    private static ServiceProvider ContainerInit(string[] args)
    {
        ServiceCollection services = [];
        
        services.AddSingleton<IUI, CLI>();
        
        services.AddSingleton(ApplicationArguments.Setup(args));
        
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

// new ImageGenerator(
//     Utility.GetRelativeFilePath("out/res"), format,
//     Utility.GetRelativeFilePath("src/JosefinSans-Regular.ttf"),
//     30, 1920, 1080,
//     Color.FromRgb(33, 0, 46),
//     (w, freq) => (
//         (byte)(freq == 1 ? 84 : freq <= 5 ? 255 : 57),
//         (byte)(freq == 1 ? 253 : freq <= 5 ? 122 : 108),
//         (byte)(freq == 1 ? 158 : freq <= 5 ? 254 : 255),
//         (byte)Math.Min(255, 55 + w.Length * 20)
//     )
// )