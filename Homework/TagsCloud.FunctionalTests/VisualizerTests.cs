using System.Collections.Generic;
using System.IO;
using Autofac;
using NUnit.Framework;
using TagsCloud.Visualization;
using TagsCloud.Visualization.ColorGenerators;
using TagsCloud.Visualization.Extensions;
using TagsCloud.Visualization.TagsCloudVisualizers;
using TagsCloud.Visualization.TextProviders;
using TagsCloud.Visualization.TextProviders.FileReaders;

namespace TagsCloud.FunctionalTests
{
    public class VisualizerTests
    {
        [TestCase("txt", TestName = "txt")]
        [TestCase("doc", TestName = "doc")]
        [TestCase("docx", TestName = "docx")]
        [TestCase("pdf", TestName = "pdf")]
        public void ShouldReadWords_From(string extension)
        {
            var settings = GenerateDefaultSettings();

            using var container = CreateContainer(settings, extension);

            var visualizer = container.Resolve<ITagsCloudVisualizer>();
            visualizer.GenerateImage(container.Resolve<ITextProvider>())
                .Then(x => x.Dispose());
        }

        private static TagsCloudModuleSettings GenerateDefaultSettings() =>
            new()
            {
                LayouterType = typeof(CircularCloudLayouter),
                ColorGenerator = new RandomColorGenerator()
            };

        private static IContainer CreateContainer(TagsCloudModuleSettings settings, string extension)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new TagsCloudModule(settings));

            builder.RegisterType<TxtFileReader>().As<IFileReader>();
            builder.RegisterType<DocFileReader>().As<IFileReader>();
            builder.RegisterType<PdfFileReader>().As<IFileReader>();

            builder.Register(ctx =>
                    new FileSystemTextProvider(Path.Combine(Directory.GetCurrentDirectory(), $"test.{extension}"),
                        ctx.Resolve<IEnumerable<IFileReader>>()))
                .As<ITextProvider>();

            return builder.Build();
        }
    }
}