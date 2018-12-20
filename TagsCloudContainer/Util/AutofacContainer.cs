using System.Collections.Generic;
using Autofac;
using System.Drawing;
using TagsCloudContainer.Cloud;
using TagsCloudVisualization;
using TagsCloudContainer.Words;
using TagsCloudContainer.Arguments;
using ResultOf;

namespace TagsCloudContainer.Util
{
    class AutofacContainer
    {
        private IContainer container;
        public TagCloud TagCloud => container.Resolve<TagCloud>();
        public Result<Brush> Brush => container.Resolve<Result<Brush>>();
        public Result<string> FontName => container.ResolveNamed<Result<string>>("FontName");
        public Result<string> OutputPath => container.Resolve<ArgumentsParser>().OutputPath;

        public AutofacContainer(string[] args)
        {
            var container = new ContainerBuilder();

            container
                .RegisterType<ArgumentsParser>()
                .AsSelf()
                .WithParameter("args", args)
                .SingleInstance();

            container
                .Register(c =>
                    new TextFileWordsReader(c.Resolve<ArgumentsParser>()
                            .InputPath)
                    .ReadWords())
                .Named<Result<string[]>>("words").SingleInstance();

            container.Register(c =>
            {
                var parser = c.Resolve<ArgumentsParser>();
                return new TextFileWordsReader(parser.WordsToExcludePath).ReadWordsInHashSet();
            })
                .Named<Result<HashSet<string>>>("WordsToExclude")
                .SingleInstance();

            container
                .Register(c => c.Resolve<ArgumentsParser>().FontName)
                .Named<Result<string>>("FontName")
                .SingleInstance();

            container.Register(c => c.Resolve<ArgumentsParser>().Brush)
                .As<Result<Brush>>()
                .SingleInstance();

            container
                .RegisterType<TagCloud>()
                .As<TagCloud>()
                .SingleInstance();

            container
                .RegisterType<SuperTightCloudLayouter>()
                .As<ICloudLayouter>()
                .WithParameter("center", new Point(1000, 1000))
                .SingleInstance();

            container.Register(c => 
                    new WordPreprocessing(c.ResolveNamed<Result<string[]>>("words"))
                        .ToLower()
                        .Exclude(c.ResolveNamed<Result<HashSet<string>>>("WordsToExclude"))
                        .IgnoreInvalidWords())
                .As<WordPreprocessing>()
                .SingleInstance();

            container.RegisterType<WordAnalizer>()
                .AsSelf()
                .SingleInstance();

            this.container = container.Build();
        }
    }
}
