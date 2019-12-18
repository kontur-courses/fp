using Autofac;
using TagsCloud.FileReader;
using CommandLine;
using TagsCloud.Interfaces;
using System;
using MyStemWrapper;
using TagsCloud.PathValidators;
using TagsCloud.Splitters;
using TagsCloud.WordStreams;
using TagsCloud.TagGenerators;
using TagsCloud.WordCounters;
using TagsCloud.WordHandlers;
using TagsCloud.WordValidators;
using TagsCloud.FontGenerators;
using TagsCloud.CloudDrawers;
using TagsCloud.ImageSavers;
using TagsCloud.TagCloudGenerators;
using TagsCloud;
using TagsCloud.ErrorHandling;

namespace TagCloudCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            var parsedArgs = Parser.Default.ParseArguments<Options>(args);

            TagCloudSettingsExtensions.GetCloudSettingsFromArguments(parsedArgs)
                .Then(RegisterAllTypes)
                .Then(container => container.Resolve<TagCloudVisualizer>())
                .Then(tagCloudVisualizer => tagCloudVisualizer.GenerateTagCloud())
                .OnFail(Console.WriteLine);
        }

        private static Result<IContainer> RegisterAllTypes(TagCloudSettings settings)
        {
            var container = new ContainerBuilder();
            TypesCollector.GetTypeGenerationLayoutersByName(settings.generationAlgorithmName)
                .Then(tagLayouterType => container.RegisterType(tagLayouterType).As<ITagCloudLayouter>().SingleInstance());
            TypesCollector.GetTypeSplitterByName(settings.splitterName)
                .Then(textSplitterType => container.RegisterType(textSplitterType).As<ITextSplitter>().SingleInstance());
            TypesCollector.GetColorSchemeByName(settings.colorSchemeName)
                .Then(colorSchemeType => container.RegisterType(colorSchemeType).As<IColorScheme>().SingleInstance());
            container.RegisterType<BoringWordStream>().AsSelf().SingleInstance();
            container.RegisterInstance(settings).AsSelf().SingleInstance();
            container.RegisterType<PathValidator>().AsSelf();
            container.RegisterType<SplitterByLine>().AsSelf();
            container.RegisterType<WordStream>().AsSelf().SingleInstance();
            container.RegisterType<TagGenerator>().As<ITagGenerator>().SingleInstance();
            container.RegisterType<TagCloudGenerator>().AsImplementedInterfaces().SingleInstance();
            container.RegisterType<DefaultCloudDrawer>().AsImplementedInterfaces().SingleInstance();
            container.RegisterType<ImageSaver>().AsImplementedInterfaces().SingleInstance();
            container.RegisterType<DictionaryBasedCounter>().AsImplementedInterfaces().SingleInstance();
            container.RegisterType<TxtReader>().AsImplementedInterfaces().AsSelf().SingleInstance();
            container.RegisterType<ToLowerWordHandler>().AsImplementedInterfaces();
            container.RegisterType<TagCloudVisualizer>().AsSelf().SingleInstance();
            container.RegisterType<WordValidatorSettings>().AsSelf().SingleInstance();
            container.RegisterType<DefaultWordValidator>().AsImplementedInterfaces().SingleInstance();
            container.RegisterType<BoringWordsValidator>().AsSelf().SingleInstance();
            container.RegisterType<SimpleFontGenerator>().AsImplementedInterfaces().SingleInstance();
            container.RegisterInstance(new MyStem { PathToMyStem = settings.pathToMystem, Parameters = "-li" })
                .AsSelf().SingleInstance();
            return container.Build().AsResult();
        }
    }
}
