using Autofac;
using TagsCloud.FileReader;
using CommandLine;
using TagsCloud.Interfaces;
using System.Drawing;
using System;
using MyStemWrapper;
using System.IO;
using TagsCloud.PathValidators;
using TagsCloud.Spliters;
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

            GetCloudSettingsFromArguments(parsedArgs)
                .Then(settings => RegisterAllTypes(settings))
                .Then(containter => containter.Resolve<TagCloudVisualizer>())
                .Then(vizualizer => vizualizer.GenerateTagCloud())
                .OnFail(Console.WriteLine);
        }

        private static Result<IContainer> RegisterAllTypes(TagCloudSettings settings)
        {
            var container = new ContainerBuilder();
            TypesCollector.GetTypeGeneationLayoutersByName(settings.generationAlgoritmName)
                .Then(tagLayouterType => container.RegisterType(tagLayouterType).As<ITagCloudLayouter>().SingleInstance());
            TypesCollector.GetTypeSpliterByName(settings.spliterName)
                .Then(textSpliterType => container.RegisterType(textSpliterType).As<ITextSpliter>().SingleInstance());
            TypesCollector.GetColorSchemeByName(settings.colorSchemeName)
                .Then(colorSchemeType => container.RegisterType(colorSchemeType).As<IColorScheme>().SingleInstance());
            container.RegisterInstance(settings).AsSelf().SingleInstance();
            container.RegisterType<PathValidator>().AsSelf();
            container.RegisterType<SpliterByLine>().AsSelf();
            container.RegisterType<WordStream>().As<IWordStream>().SingleInstance();
            container.RegisterType<TagGenerator>().As<ITagGenerator>().SingleInstance();
            container.RegisterType<TagCloudGenerator>().AsImplementedInterfaces().SingleInstance();
            container.RegisterType<DefaultCloudDrawer>().AsImplementedInterfaces().SingleInstance();
            container.RegisterType<ImageSaver>().AsImplementedInterfaces().SingleInstance();
            container.RegisterType<DictionaryBasedCounter>().AsImplementedInterfaces().SingleInstance();
            container.RegisterType<TxtReader>().AsImplementedInterfaces().SingleInstance();
            container.RegisterType<ToLowerWordHandler>().AsImplementedInterfaces();
            container.RegisterType<TagCloudVisualizer>().AsSelf().SingleInstance();
            container.RegisterType<WordValidatorSettings>().AsSelf().SingleInstance();
            container.RegisterType<DefaultWordValidator>().AsImplementedInterfaces().SingleInstance();
            container.RegisterType<SimpleFontGenerator>().AsImplementedInterfaces().SingleInstance();
            container.RegisterInstance(new MyStem { PathToMyStem = settings.pathToMystem, Parameters = "-li" })
                .AsSelf().SingleInstance();
            return container.Build().AsResult();
        }

        private static Result<TagCloudSettings> GetCloudSettingsFromArguments(ParserResult<Options> parserResult)
        {
            Result<TagCloudSettings> settings = null;
            parserResult
              .WithParsed(opts =>
              {
                  settings = TypesCollector.GetFormatFromPathSaveFile(opts.savePath)
                  .Then(imageFormatResult => new TagCloudSettings(opts.InputFiles,
                          opts.savePath,
                          opts.boringWordsPath,
                          opts.width,
                          opts.height,
                          Color.FromName(opts.backgroundColor),
                          opts.fontName,
                          opts.ignoredPartsOfSpeech.Split(",", StringSplitOptions.RemoveEmptyEntries),
                          opts.GenerationAlghoritm,
                          opts.splitType,
                          opts.ColorScheme,
                          Path.Combine(Path.GetFullPath(Path.Combine("Resources", "mystem.exe"))),
                          imageFormatResult));
              })
              .WithNotParsed(o => settings=Result.Fail<TagCloudSettings>("Wrong comandline argument."));
            return settings
                .Then(ValidateIsKnownColor)
                .Then(ValidateImageSize)
                .Then(ValidateMystemLocation);
        }

        private static Result<TagCloudSettings> ValidateIsKnownColor(TagCloudSettings settings)
        {
            return Validate(settings, settings => settings.backgroundColor.IsKnownColor, $"Unknown color {settings.backgroundColor.Name}");
        }

        private static Result<TagCloudSettings> ValidateImageSize(TagCloudSettings settings)
        {
            return Validate(settings, settings => settings.widthOutputImage > 0 && settings.heightOutputImage > 0, 
                $"The height and width of the resulting image must be greater than 0");
        }

        private static Result<TagCloudSettings> ValidateMystemLocation(TagCloudSettings settings)
        {
            return Validate(settings, settings => File.Exists(settings.pathToMystem),
                $"Mystem not found. Expected location is {settings.pathToMystem}");
        }

        private static Result<T> Validate<T>(T obj, Func<T, bool> predicate, string errorMessage)
        {
            return predicate(obj)
                ? Result.Ok(obj)
                : Result.Fail<T>(errorMessage);
        }
    }
}
