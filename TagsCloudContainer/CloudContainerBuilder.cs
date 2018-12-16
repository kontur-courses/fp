using System.Collections.Generic;
using System.Drawing;
using Autofac;
using TagsCloudContainer.ResultRenderer;
using TagsCloudContainer.WordFormatters;
using TagsCloudContainer.WordLayouts;
using TagsCloudContainer.WordsPreprocessors;
using TagsCloudContainer.WordsReaders;

namespace TagsCloudContainer
{
    public class CloudContainerBuilder
    {
        public IContainer BuildTagsCloudContainer()
        {
            var builder = new ContainerBuilder();

            builder.Register(z => new Config())
                .AsSelf();

            builder.RegisterType<ImageRenderer>()
                .As<IResultRenderer>();

            builder.RegisterType<CustomBoringWordsRemover>()
                .As<IWordsPreprocessor>();

            builder.RegisterType<TxtReader>()
                .As<IWordsReader>()
                .SingleInstance();

            builder.Register(z => new SimpleFormatter(
                    z.Resolve<IWordsWeighter>()))
                .As<IWordFormatter>();

            builder.RegisterType<CircularCloudLayouter>()
                .As<ILayouter>();

            builder.RegisterType<WordsWeighter>()
                .As<IWordsWeighter>()
                .SingleInstance();

            builder
                .RegisterTypes(typeof(WordsLower), typeof(BoringWordsRemover))
                .As<IWordsPreprocessor>()
                .SingleInstance();

            builder.RegisterType<WordsSizer>()
                .AsSelf();

            builder.RegisterType<TagsCloudBuilder>()
                .AsSelf();

            return builder.Build();
        }
    }
}