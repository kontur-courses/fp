using System;
using System.Linq;
using Autofac;
using CommandLine;
using ResultMonad;
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
            var settings = new SettingProvider().GetSettings(options);

            settings.AsResult()
                .Then(CreateContainer)
                .Then(container => container.Resolve<Visualizer>().Visualize())
                .Then(image => new ImageSaver(settings.Saver.Directory, settings.Saver.ImageName).Save(image))
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