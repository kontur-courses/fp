using Autofac;
using System.Drawing;
using ResultPattern;
using TagsCloudConsoleUI.DIPresetModules;
using TagsCloudGenerator;

namespace TagsCloudConsoleUI
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            ConsoleManager.Run(new DefaultConsoleFormatter(), OnCallAction);
        }

        private static IContainer BuildContainer(BuildOptions options)
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule(new CircularRandomCloudModule(options));
            builder.RegisterModule(new WordParserWithYandexToolModule(options));
            builder.RegisterModule(new BitmapImageCreatorModule(options));
            builder.RegisterModule(new CloudSettingsSetupModule(options));

            builder.RegisterInstance(options.InputFilePath).Named<string>("filepath");

            return builder.Build();
        }

        private static Result<Bitmap> OnCallAction(ConsoleParsedOptions parsedOptions)
        {
            var resultOptions = Result.Of(() => new BuildOptions(parsedOptions));
            if (!resultOptions.IsSuccess)
                return Result.Fail<Bitmap>(resultOptions.Error);
            
            var options = resultOptions.GetValueOrThrow();

            return resultOptions
                .Then(BuildContainer)
                .Then(container =>
                {
                    var fullPath = options.InputFilePath;
                    var size = new Size(options.Width, options.Height);
                    var format = container.Resolve<CloudSettings>();

                    return container.Resolve<CloudBuilder<Bitmap>>()
                        .CreateTagCloudRepresentation(fullPath, size, format);
                })
                .Then(image => Result.Of(() =>
                {
                    image.Save(options.OutputFilePath, ImageFormatter.ParseImageFormat(options.ImageExtension));
                    return image;
                }));
        }
    }
}
