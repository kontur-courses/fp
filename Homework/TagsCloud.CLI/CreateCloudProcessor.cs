using System;
using System.Collections.Generic;
using Autofac;
using TagsCloud.Visualization;
using TagsCloud.Visualization.Extensions;
using TagsCloud.Visualization.WordsFilter;
using TagsCloud.Visualization.WordsReaders;
using TagsCloud.Visualization.WordsReaders.FileReaders;
using TagsCloud.Words.Options;

namespace TagsCloud.Words
{
    public static class CreateCloudProcessor
    {
        public static int Run(CreateCloudCommand options)
        {
            try
            {
                var result = SettingsCreator.CreateFrom(options)
                    .OnFail(x => Console.WriteLine(x, Console.Error))
                    .Then(x =>
                    {
                        using var container = CreateContainer(x).BeginLifetimeScope();

                        container.Resolve<CliTagsCloudVisualizer>().Run();
                    });

                return result.IsSuccess ? 0 : 1;
            }
            catch (Exception _)
            {
                return 1;
            }
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