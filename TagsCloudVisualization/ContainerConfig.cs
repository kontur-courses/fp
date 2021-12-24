using System;
using System.IO;
using System.Reflection;
using Autofac;
using Autofac.Core;
using NHunspell;
using TagsCloudVisualization.Commands;
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
using TagsCloudVisualization.Processors;

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
                .And(builder =>
                        TryRegisterHunspellStemer(builder, executingPath + DictRuAff, executingPath + DictRuDic),
                    "не удалось зарегистрировать обработчик слов HunspellStemer.")
                .And(builder => builder.RegisterType<PronounFilter>().As<IWordFilter>(),
                    "не удалось зарегистрировать фильтр местоимений PronounFilter.")
                .And(builder => TryRegisterCustomFilter(builder, executingPath + DictExcludeWords),
                    "не удалось зарегистрировать фильтр слов CustomFilter.")
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
                .And(builder => builder.RegisterType<CreateCloudProcessor>().SingleInstance(),
                    "не удалось зарегистрировать обработчик CreateCloudProcessor.")
                .And(builder => builder.RegisterType<ShowDemoProcessor>().SingleInstance(),
                    "не удалось зарегистрировать обработчик ShowDemoProcessor.")
                .Then(builder => builder.Build());

            return containerBuilder;
        }

        private static void TryRegisterHunspellStemer(ContainerBuilder builder, string affFile, string dicFile)
        {
            Result.Of(() => new Hunspell(affFile, dicFile))
                .RefineError("Не удалось зарегистрировать обработчик слов Hunspell:")
                .OnFail(Console.WriteLine)
                .OnSuccess(hunspell =>
                {
                    builder.RegisterInstance(hunspell).SingleInstance();
                    builder.RegisterType<HunspellStemer>().As<IStemer>();
                });
        }

        private static void TryRegisterCustomFilter(ContainerBuilder builder, string filterFile)
        {
            new TextFileReader().ReadLines(filterFile)
                .Then(words => new CustomFilter(words))
                .RefineError("Не удалось зарегистрировать фильтр слов:")
                .OnFail(Console.WriteLine)
                .OnSuccess(filter => builder.RegisterInstance(filter).As<IWordFilter>());
        }
    }
}