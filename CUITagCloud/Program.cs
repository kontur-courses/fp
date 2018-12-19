using System.Collections.Generic;
using System.Drawing;
using Autofac;
using CommandLine;
using TagsCloudContainer;
using TagsCloudContainer.CloudBuilder;
using TagsCloudContainer.CloudDrawers;
using TagsCloudContainer.CloudLayouters;
using TagsCloudContainer.CloudLayouters.PointGenerators;
using TagsCloudContainer.CloudTagController;
using TagsCloudContainer.FileReaders;
using TagsCloudContainer.Settings;
using TagsCloudContainer.TextParsers;
using TagsCloudContainer.WordConverter;

namespace CUITagCloud
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Option>(args).WithParsed(DrawCloud);
        }

        private static void DrawCloud(Option options)
        {
            var container = BuildContainer(options);
            var cloudTagController = container.Resolve<ICloudTagController>();

            cloudTagController.Work();
        }

        private static IContainer BuildContainer(Option options)
        {
            var builder = new ContainerBuilder();

            builder.RegisterInstance(options).As<Option>();

            builder.RegisterType<ImageSettings>();
            builder.RegisterType<FileSettings>();
            builder.RegisterType<FilterSettings>();
            builder.RegisterType<TextSettings>();

            builder.RegisterType<TextFileReader>().As<IFileReader>();
            builder.RegisterType<InitialFormWordConverter>().As<IWordConverter>();
            builder.RegisterType<TextParser>().As<ITextParser>();
            builder.RegisterType<TagsCloudBuilder>().As<ICloudBuilder>();
            builder.RegisterType<CircularCloudLayouter>().As<ICloudLayouter>();
            builder.RegisterType<CloudDrawer>().As<ICloudDrawer>();
            builder.RegisterType<ArchimedesSpiralPointGenerator>().As<IEnumerable<Point>>();
            builder.RegisterType<CloudTagController>().As<ICloudTagController>();

            return builder.Build();
        }
    }
}