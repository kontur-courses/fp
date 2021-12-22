﻿using CTV.Common;
using CTV.Common.Layouters;
using CTV.Common.Layouters.Spirals;
using CTV.Common.Preprocessors;
using Microsoft.Extensions.DependencyInjection;

namespace CTV.Common.VisualizerContainer
{
    public static class VisualizerContainerFactory
    {
        public static ServiceProvider CreateInstance(VisualizerFactorySettings factorySettings)
        {
            var container = new ServiceCollection();
            InitCommonObjects(container);
            InitImageSaver(container, factorySettings);
            InitFileReader(container, factorySettings);
            InitVisualizerSettings(container, factorySettings);
            return container.BuildServiceProvider();
        }

        private static void InitCommonObjects(IServiceCollection container)
        {
            container.AddScoped<Visualizer>();
            container.AddScoped<IFileStreamFactory, FileStreamFactory>();
            container.AddScoped<IWordsParser, RussianWordsParser>();
            container.AddScoped<ToLowerPreprocessor>();
            container.AddScoped<IHunspeller, NHunspeller>();
            container.AddScoped<IHunspellerFilesProvider, RussianHunspellerFilesProvider>();
            container.AddScoped<RemovingBoringWordsPreprocessor>();
            container.AddScoped<IWordsPreprocessor, CombinedPreprocessor>(
                provider => new CombinedPreprocessor(
                    new IWordsPreprocessor[]
                    {
                        provider.GetService<ToLowerPreprocessor>(),
                        provider.GetService<RemovingBoringWordsPreprocessor>()
                    }));
            container.AddScoped<ILayouter, CircularCloudLayouter>();
            container.AddScoped<ISpiral, ExpandingSquareSpiral>();
            container.AddScoped<IWordSizer, FrequencyBasedWordSizer>();
        }

        private static void InitImageSaver(IServiceCollection container, VisualizerFactorySettings factorySettings)
        {
            container.AddSingleton(factorySettings.SavingFormat.ToImageSaver());
        }

        private static void InitFileReader(IServiceCollection container, VisualizerFactorySettings factorySettings)
        {
            container.AddSingleton(factorySettings.InputFileFormat.ToFileReader());
        }

        private static void InitVisualizerSettings(IServiceCollection container, VisualizerFactorySettings settings)
        {
            var visualizerSettings = new VisualizerSettings(
                settings.ImageSize,
                settings.TextFont,
                settings.TextColor,
                settings.BackgroundColor,
                settings.StrokeColor
            );
            
            container.AddSingleton(visualizerSettings);
        }
    }
}