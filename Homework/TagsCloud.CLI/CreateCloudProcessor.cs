using System;
using System.Collections.Generic;
using Autofac;
using TagsCloud.Visualization;
using TagsCloud.Visualization.Extensions;
using TagsCloud.Visualization.TextProviders;
using TagsCloud.Visualization.TextProviders.FileReaders;
using TagsCloud.Visualization.WordsFilters;
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
            catch (Exception)
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

            builder.Register(ctx => new FileSystemTextProvider(settings.InputWordsFile,
                    ctx.Resolve<IEnumerable<IFileReader>>()))
                .As<ITextProvider>();

            RegisterBoringWordsFilter(builder, settings);

            builder.RegisterType<CliTagsCloudVisualizer>().AsSelf();
            return builder.Build();
        }


        private static void RegisterBoringWordsFilter(ContainerBuilder builder, TagsCloudModuleSettings settings)
        {
            if (settings.BoringWordsFile == null)
                builder.Register(_ => new BoringWordsFilter()).As<IWordsFilter>();
            else
                builder.Register(ctx =>
                    {
                        var fileReader = new FileSystemTextProvider(settings.BoringWordsFile,
                            ctx.Resolve<IEnumerable<IFileReader>>());

                        var boringWords = fileReader.Read()
                            .Then(x => new BoringWordsFilter(x));

                        return boringWords.IsSuccess ? boringWords.GetValueOrThrow() : new BoringWordsFilter();
                    }
                ).As<IWordsFilter>().SingleInstance();
        }
    }
}