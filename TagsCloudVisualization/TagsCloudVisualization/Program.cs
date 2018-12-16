using System;
using System.Windows.Forms;
using Autofac;

namespace TagsCloudVisualization
{
    class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ConsoleApplication>().AsSelf();
            builder.RegisterType<ConsoleApplicationRunner>().As<IApplicationRunner>();
            builder.RegisterType<TxtReader>().As<IFileReader>();
            builder.RegisterType<NWordSizer>().As<ISizeDefiner>();
            builder.RegisterType<CircularCloudLayouter>().As<ICloudLayouter>();
            builder.RegisterType<Visualizer>().As<IVisualizer>();
            builder.RegisterType<MonochromePalette>().As<IWordPalette>();
            var container = builder.Build();

            var runner = container.Resolve<IApplicationRunner>();

            runner.Run(args);
        }
    }
}
