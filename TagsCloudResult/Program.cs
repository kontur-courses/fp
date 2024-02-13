using Microsoft.Extensions.DependencyInjection;
using ResultSharp;
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
        var appArgsResult = ApplicationArguments.Setup(args);
        if (appArgsResult.IsErr)
        {
            Console.WriteLine(appArgsResult.UnwrapErr());
            return;
        }

        var fontFamilyResult = FontFinder.TryGetFont(appArgsResult.Unwrap());
        if (fontFamilyResult.IsErr)
        {
            Console.WriteLine(fontFamilyResult.UnwrapErr());
            return;
        }

        using var container = DIContainer.ContainerInit(appArgsResult.Unwrap());

        var app = container.GetService<Application>()!;

        app.Run(container.GetService<ApplicationArguments>()!);
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