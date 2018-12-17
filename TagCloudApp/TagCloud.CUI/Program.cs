using System;
using Autofac;
using Autofac.Core;
using CommandLine;
using TagCloud.Core.Layouters;
using TagCloud.Core.Painters;
using TagCloud.Core.Settings.DefaultImplementations;
using TagCloud.Core.Settings.Interfaces;
using TagCloud.Core.Util;
using TagCloud.Core.Visualizers;
using TagCloud.Core.WordsParsing;
using TagCloud.Core.WordsParsing.WordsProcessing;
using TagCloud.Core.WordsParsing.WordsProcessing.WordsProcessingUtilities;
using TagCloud.Core.WordsParsing.WordsReading;

namespace TagCloud.CUI
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            InjectDependencies(builder);

            if (args.Length == 0)
                args = new[]
                {
                    "-p", "test_words.txt",

                    // Each line below leads to an error:
                    //"-w", "-800",
                    //"-h", "-600",
                    //"--spiralstep", "-1",
                    //"--maxtagscount", "-10",
                    //"--minfontsize", "-100",
                    //"--maxfontsize", "-100",
                    "-i", "result_image.wrong_format",
                    //"-b", "boring_words.wrong_format",
                    //"-b", "nonexistence_boring_words.txt",
                    //"-f", "wrong_font",
                    //"--backgroundcolor", "asdf",
                    //"--tagbrush", "dfasdf",
                };

            Parser.Default
                .ParseArguments<Options>(args)
                .WithParsed(options =>
                {
                    builder.RegisterInstance(options)
                        .As<IPaintingSettings>()
                        .As<IVisualizingSettings>()
                        .As<ITagCloudSettings>()
                        .As<ITextParsingSettings>()
                        .As<ILayoutingSettings>();
                });

            Result.Of(() => builder.Build())
                .Then(container => container.Resolve<Core.TagCloud>())
                .Then(tagCloud => tagCloud.MakeTagCloudAndSave())
                .OnFail(Console.WriteLine);
        }

        private static void InjectDependencies(ContainerBuilder builder)
        {
            builder.RegisterType<Core.TagCloud>().AsSelf();

            builder.RegisterType<TxtWordsReader>().As<IWordsReader>();
            builder.RegisterType<XmlWordsReader>().As<IWordsReader>();
            builder.RegisterType<GeneralWordsReader>().AsSelf();
            builder.RegisterType<LowerCaseUtility>().As<IWordsProcessingUtility>();
            builder.RegisterType<SimpleWordsProcessor>().As<IWordsProcessor>();
            builder.RegisterType<TextParsingSettings>().As<ITextParsingSettings>().AsSelf().SingleInstance();
            builder.RegisterType<WordsParser>()
                .WithParameter(new ResolvedParameter(
                    (pi, ctx) => pi.ParameterType == typeof(IWordsReader),
                    (pi, ctx) => ctx.Resolve<GeneralWordsReader>()))
                .AsSelf();

            builder.RegisterType<SimpleTagCloudVisualizer>().As<ITagCloudVisualizer>();
            builder.RegisterType<CircularCloudLayouter>().As<ICloudLayouter>();
            builder.RegisterType<OneColorPainter>().As<IPainter>();
        }
    }
}