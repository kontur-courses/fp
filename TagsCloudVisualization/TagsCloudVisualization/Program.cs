using System;
using System.Windows.Forms;
using Autofac;
using TagsCloudVisualization.App.Cloud.Words;

namespace TagsCloudVisualization
{
    class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            GetBuilder()
                .Build()
                .Resolve<IApplicationRunner>()
                .Run(args);
        }

        private static ContainerBuilder GetBuilder()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ConsoleApplication>().AsSelf();
            builder.RegisterType<ConsoleApplicationRunner>().As<IApplicationRunner>();
            builder.RegisterType<TxtReader>().As<IFileReader>();
            builder.RegisterType<NWordSizer>().As<ISizeDefiner>();
            builder.RegisterType<CircularCloudLayouter>().As<ICloudLayouter>();
            builder.RegisterType<Visualizer>().As<IVisualizer>();
            builder.RegisterType<MonochromePalette>().As<IWordPalette>();
            builder.RegisterType<WordCounter>().As<IWordCounter>();
            builder.RegisterType<ImageSaver>().As<IImageSaver>();

            return builder;
        }
    }
}
