using System;
using System.Linq;
using Autofac;
using CommandLine;
using ResultMonad;
using ResultMonad.Extensions;
using TagsCloudImageSaver;
using TagsCloudVisualization;
using TagsCloudVisualization.Settings;

namespace TagsCloudCLI
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var result = Parser.Default.ParseArguments<Options>(args);
            if (result.Errors.Any())
                return;
            Run(result.Value);
        }

        private static void Run(Options options)
        {
            var generalSettings = new SettingProvider().GetSettings(options);
            
            generalSettings
                .SelectMany(CreateContainer, (settings, container) => new { settings, container })
                .Then(tuple =>
                {
                    var image = tuple.container.Resolve<Visualizer>().Visualize();
                    new ImageSaver(tuple.settings.Saver.Directory, tuple.settings.Saver.ImageName).Save(image);
                })
                .OnFail(Console.WriteLine);
        }

        private static Result<IContainer> CreateContainer(GeneralSettings settings)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new TagsCloudModule(settings));
            var container = builder.Build();
            return container.AsResult();
        }
    }
}