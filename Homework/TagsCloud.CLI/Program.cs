using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using CommandLine;
using TagsCloud.Visualization;
using TagsCloud.Visualization.Extensions;
using TagsCloud.Visualization.WordsFilter;
using TagsCloud.Visualization.WordsReaders;
using TagsCloud.Visualization.WordsReaders.FileReaders;

namespace TagsCloud.Words
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var result = Parser.Default.ParseArguments<Options>(args);
            if (result.Errors.Any())
                return;

            new SettingsCreator()
                .Parse(result.Value)
                .OnFail(x => Console.WriteLine(x, Console.Error))
                .Then(s =>
                {
                    using var container = CreateContainer(s).BeginLifetimeScope();

                    container.Resolve<CliTagsCloudVisualizer>().Run();
                });
        }

        private static IContainer CreateContainer(TagsCloudModuleSettings settings)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new TagsCloudModule(settings));

            builder.RegisterType<TxtFileReader>().As<IFileReader>();
            builder.RegisterType<DocFileReader>().As<IFileReader>();
            builder.RegisterType<PdfFileReader>().As<IFileReader>();

            builder.Register(ctx => new FileProvider(settings.InputWordsFile,
                    ctx.Resolve<IEnumerable<IFileReader>>()))
                .As<IWordsProvider>();
            RegisterBoringWordsFilter(builder, settings);

            builder.RegisterType<CliTagsCloudVisualizer>().AsSelf();
            return builder.Build();
        }


        private static void RegisterBoringWordsFilter(ContainerBuilder builder, TagsCloudModuleSettings settings)
        {
            if (settings.BoringWordsFile == null)
                builder.Register(_ => new BoringWordsFilter()).As<IWordsFilter>();
            else
                builder.Register(ctx => new BoringWordsFilter(new FileProvider(settings.BoringWordsFile,
                    ctx.Resolve<IEnumerable<IFileReader>>()))).As<IWordsFilter>();
        }
    }
}