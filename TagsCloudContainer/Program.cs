using System;
using System.Linq;
using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using TagsCloudContainer.CloudLayouter;
using TagsCloudContainer.Drawing;
using TagsCloudContainer.FileReaders;
using TagsCloudContainer.ImageSavers;
using TagsCloudContainer.Settings;
using TagsCloudContainer.WordFilters;
using TagsCloudContainer.WordsConverter;
using TagsCloudContainer.WordsPreprocessors;

namespace TagsCloudContainer
{
    static class Program
    {
        static void Main(string[] args)
        {
            var parsedArgs = Parser.Default.ParseArguments<AppSettings>(args);
            if (parsedArgs.Errors.Any())
            {
                Environment.Exit(-1);
            }
            var appSettings = parsedArgs.Value;
            
            var container = GetConfiguredContainer(appSettings);
            var tagCloudCreator = container.GetService<TagCloudCreator>();
            tagCloudCreator.CreateTagCloudImage();
        }

        private static ServiceProvider GetConfiguredContainer(IAppSettings appSettings)
        {
            var container = new ServiceCollection();
            container.AddSingleton<IFileReader, TxtFileReader>();
            container.AddSingleton<IFileReader, DocFileReader>();
            container.AddSingleton<IFileReaderFactory, FileReaderFactory>();
            container.AddSingleton<IWordsPreprocessor, WordsPreprocessor>();
            container.AddSingleton<IDrawer, TagCloudDrawer>();
            container.AddSingleton<IWordsFilter, BoringWordsFilter>();
            container.AddSingleton<IWordConverter, WordToTagConverter>();
            container.AddSingleton<IImageSaver, ImageSaver>();
            container.AddSingleton<TagCloudCreator, TagCloudCreator>();
            container.AddSingleton<ICloudLayouter, CircularCloudLayouter>();
            container.AddSingleton<ISpiral, ArchimedeanSpiral>();
            container.AddSingleton<IFontCreator, FontCreator>();
            container.AddSingleton(appSettings);

            return container.BuildServiceProvider();
        }
    }
}