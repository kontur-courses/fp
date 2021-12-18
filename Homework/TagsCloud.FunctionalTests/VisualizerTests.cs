using System.Collections.Generic;
using System.IO;
using Autofac;
using NUnit.Framework;
using TagsCloud.Visualization;
using TagsCloud.Visualization.ContainerVisitor;
using TagsCloud.Visualization.LayouterCores;
using TagsCloud.Visualization.Utils;
using TagsCloud.Visualization.WordsReaders;
using TagsCloud.Visualization.WordsReaders.FileReaders;

namespace TagsCloud.FunctionalTests
{
    public class VisualizerTests
    {
        [TestCase("txt", TestName = "txt")]
        [TestCase("doc", TestName = "doc")]
        [TestCase("docx", TestName = "docx")]
        [TestCase("pdf", TestName = "pdf")]
        public void Should_ReadWords_From(string extension)
        {
            var settings = GenerateDefaultSettings();
            
            using var container = CreateContainer(settings, extension);

            var visualizer = container.Resolve<ILayouterCore>();
            visualizer.GenerateImage(container.Resolve<IWordsReadService>())
                .Then(x => x.Dispose());
        }

        private TagsCloudModuleSettings GenerateDefaultSettings() =>
            new()
            {
                LayouterType = typeof(CircularCloudLayouter),
                LayoutVisitor = new RandomColorDrawerVisitor()
            };

        private IContainer CreateContainer(TagsCloudModuleSettings settings, string extension)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new TagsCloudModule(settings));
            
            builder.RegisterType<TxtFileReader>().As<IFileReader>();
            builder.RegisterType<DocFileReader>().As<IFileReader>();
            builder.RegisterType<PdfFileReader>().As<IFileReader>();
            
            builder.Register(ctx => 
                    new FileReadService(Path.Combine(Directory.GetCurrentDirectory(), $"test.{extension}"),
                        ctx.Resolve<IEnumerable<IFileReader>>()))
                .As<IWordsReadService>();

            return builder.Build();
        }
    }
}