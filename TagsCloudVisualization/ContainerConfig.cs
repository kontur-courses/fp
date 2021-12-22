using System.IO;
using System.Reflection;
using Autofac;
using Autofac.Core;
using NHunspell;
using TagsCloudVisualization.Common.ErrorHandling;
using TagsCloudVisualization.Common.FileReaders;
using TagsCloudVisualization.Common.ImageWriters;
using TagsCloudVisualization.Common.Layouters;
using TagsCloudVisualization.Common.Settings;
using TagsCloudVisualization.Common.Stemers;
using TagsCloudVisualization.Common.TagCloudPainters;
using TagsCloudVisualization.Common.Tags;
using TagsCloudVisualization.Common.TextAnalyzers;
using TagsCloudVisualization.Common.WordFilters;

namespace TagsCloudVisualization
{
    public static class ContainerConfig
    {
        private const string DictRuAff = @"\dicts\ru.aff";
        private const string DictRuDic = @"\dicts\ru.dic";
        private const string DictExcludeWords = @"\filters\excludeWords.txt";

        public static Result<IContainer> ConfigureContainer()
        {
            var executingPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var containerBuilder = Result.Of(() => new ContainerBuilder())
                .And(builder => builder.RegisterType<TextFileReader>().As<IFileReader>().AsSelf(),
                    "не удалось зарегистрировать обработчик текстовых документов TextFileReader.")
                .And(builder => builder
                        .RegisterInstance(new Hunspell(executingPath + DictRuAff, executingPath + DictRuDic))
                        .SingleInstance(),
                    "не удалось зарегистрировать словарь Hunspell.")
                .And(builder => builder.RegisterType<HunspellStemer>().As<IStemer>(),
                    "не удалось зарегистрировать обработчик слов HunspellStemer.")
                .And(builder => builder.RegisterType<PronounFilter>().As<IWordFilter>(),
                    "не удалось зарегистрировать фильтр местоимений PronounFilter.")
                .And(builder =>
                        builder.RegisterInstance(
                                new CustomFilter(new TextFileReader().ReadLines(executingPath + DictExcludeWords)
                                    .GetValueOrThrow()))
                            .As<IWordFilter>(),
                    "не удалось зарегистрировать фильтр слов CustomFilter.")
                // .And(builder => builder.RegisterType<CustomFilter>()
                //         .WithParameter(new ResolvedParameter(
                //             (pi, ctx) => pi.ParameterType == typeof(IEnumerable<string>),
                //             (pi, ctx) =>
                //                 ctx.Resolve<TextFileReader>().ReadLines(executingPath + DictExcludeWords).GetValueOrThrow()))
                //         .As<IWordFilter>(),
                //     "Не удалось зарегистрировать фильтр слов CustomFilter.")
                .And(builder => builder.RegisterType<ComposeFilter>()
                        .WithParameter(new ResolvedParameter(
                            (pi, ctx) => pi.ParameterType == typeof(IWordFilter[]),
                            (pi, ctx) => ctx.Resolve<IWordFilter[]>())),
                    "не удалось зарегистрировать обработчик фильтров ComposeFilter.")
                .And(builder => builder.RegisterType<TextAnalyzer>()
                        .As<ITextAnalyzer>()
                        .WithParameter(new ResolvedParameter(
                            (pi, ctx) => pi.ParameterType == typeof(IWordFilter),
                            (pi, ctx) => ctx.Resolve<ComposeFilter>())),
                    "не удалось зарегистрировать анализатор текста TextAnalyzer.")
                .And(builder => builder.RegisterType<TagBuilder>().As<ITagBuilder>(),
                    "не удалось зарегистрировать обработчик тегов TagBuilder.")
                .And(builder => builder.RegisterType<CircularCloudLayouter>().As<ILayouter>(),
                    "не удалось зарегистрировать алгоритм построения облака CircularCloudLayouter.")
                .And(builder => builder.RegisterType<TagCloudPainter>().As<ITagCloudPainter>(),
                    "не удалось зарегистрировать TagCloudPainter.")
                .And(builder => builder.RegisterType<CanvasSettings>().As<ICanvasSettings>().SingleInstance(),
                    "не удалось зарегистрировать настройки холста изображения CanvasSettings.")
                .And(builder => builder.RegisterType<TagStyleSettings>().As<ITagStyleSettings>().SingleInstance(),
                    "не удалось зарегистрировать настройки тегов TagStyleSettings.")
                .And(builder => builder.RegisterType<ImageWriter>().As<IImageWriter>(),
                    "не удалось зарегистрировать обработчик изображений ImageWriter.")
                .Then(builder => builder.Build());

            return containerBuilder;
        }
    }
}