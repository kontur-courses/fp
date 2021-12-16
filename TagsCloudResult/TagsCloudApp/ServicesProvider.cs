using System;
using Microsoft.Extensions.DependencyInjection;
using TagsCloudApp.Actions;
using TagsCloudApp.Parsers;
using TagsCloudApp.Settings;
using TagsCloudApp.WordsLoading;
using TagsCloudContainer;
using TagsCloudContainer.ColorMappers;
using TagsCloudContainer.DependencyInjection;
using TagsCloudContainer.Layout;
using TagsCloudContainer.MathFunctions;
using TagsCloudContainer.Preprocessing;
using TagsCloudContainer.Rendering;
using TagsCloudContainer.Settings;
using TagsCloudContainer.Settings.Default;
using TagsCloudVisualization;

namespace TagsCloudApp
{
    public class ServicesProvider
    {
        private readonly RenderArgs renderArgs;

        public ServicesProvider(RenderArgs renderArgs)
        {
            this.renderArgs = renderArgs;
        }

        public IServiceProvider BuildProvider()
        {
            var collection = new ServiceCollection();

            AddSettings(collection);
            AddParsers(collection);
            AddResolvers(collection);
            AddActions(collection);

            return collection
                .AddSingleton<IWordColorMapper, StaticWordColorMapper>()
                .AddSingleton<IWordColorMapper, RandomWordColorMapper>()
                .AddSingleton<IWordColorMapper, SpeechPartWordColorMapper>()
                .AddSingleton<IWordsPreprocessor, LowerCaseWordsPreprocessor>()
                .AddSingleton<IWordsPreprocessor, SpeechPartWordsFilter>()
                .AddSingleton<IFileTextLoader, TxtFileTextLoader>()
                .AddSingleton<IMathFunction, LinearFunction>()
                .AddSingleton<IMathFunction, LnFunction>()
                .AddSingleton(
                    s => s.GetRequiredService<IServiceResolver<TagsCloudLayouterType, ITagsCloudLayouter>>()
                        .GetService(TagsCloudLayouterType.FontBased))
                .AddSingleton<IWordsProvider, FileWordsProvider>()
                .AddSingleton<IBitmapSaver, BitmapSaver>()
                .AddSingleton<ITagsCloudLayouter, FontBasedLayouter>()
                .AddSingleton<IFontSizeSelector, FrequencyFontSizeSelector>()
                .AddSingleton<ICloudLayouter, CircularCloudLayouter>()
                .AddSingleton<ITagsCloudRenderer, TagsCloudRenderer>()
                .AddSingleton<ITagsCloudDirector, TagsCloudDirector>()
                .AddSingleton<Random>()
                .AddSingleton<IRenderArgs>(_ => renderArgs)
                .BuildServiceProvider();
        }

        private static void AddSettings(IServiceCollection collection)
        {
            collection.AddSingleton<IWordsScaleSettings, WordsScaleSettings>()
                .AddSingleton<IFontFamilySettings, FontFamilySettings>()
                .AddSingleton<IRenderingSettings, RenderingSettings>()
                .AddSingleton<IFontSizeSettings, FontSizeSettings>()
                .AddSingleton<ISpeechPartFilterSettings, SpeechPartFilterSettings>()
                .AddSingleton<IDefaultColorSettings, DefaultColorSettings>()
                .AddSingleton<ISpeechPartColorMapSettings, SpeechPartColorMapSettings>()
                .AddSingleton<IWordColorMapperSettings, WordColorMapperSettings>()
                .AddSingleton<ISaveSettings, SaveSettings>();
        }

        private static void AddParsers(IServiceCollection collection)
        {
            collection.AddSingleton<IKeyValueParser, KeyValueParser>()
                .AddSingleton<IWordsParser, WordsParser>()
                .AddSingleton<IArgbColorParser, ArgbColorParser>()
                .AddSingleton<IImageFormatParser, ImageFormatParser>()
                .AddSingleton<IWordSpeechPartParser, WordSpeechPartParser>()
                .AddSingleton<IEnumParser, EnumParser>();
        }

        private static void AddResolvers(IServiceCollection collection)
        {
            collection.AddSingleton(typeof(IServiceResolver<,>), typeof(ServiceResolver<,>))
                .AddSingleton(typeof(Lazy<>), typeof(LazyResolver<>))
                .AddSingleton<IFileTextLoaderResolver, FileTextLoaderResolver>();
        }

        private static void AddActions(IServiceCollection collection)
        {
            collection.AddSingleton<RenderAction>()
                .AddSingleton<IAction, SetDefaultColorAction>()
                .AddSingleton<IAction, SetFontFamilyAction>()
                .AddSingleton<IAction, SetFontSizeAction>()
                .AddSingleton<IAction, SetRenderingSettingsAction>()
                .AddSingleton<IAction, SetSaveSettingsAction>()
                .AddSingleton<IAction, SetSpeechPartColorMapAction>()
                .AddSingleton<IAction, SetSpeechPartFilterAction>()
                .AddSingleton<IAction, SetWordColorMapperAction>()
                .AddSingleton<IAction, SetWordsScaleFunctionAction>();
        }
    }
}