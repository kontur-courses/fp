using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Autofac;
using TagsCloud.CloudLayouters;
using TagsCloud.Core;
using TagsCloud.FileReader;
using TagsCloud.Spirals;
using TagsCloud.Visualization;

namespace TagsCloud.ClientGUI
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(BuildContainer().Resolve<MainForm>());
        }

        private static IContainer BuildContainer()
        {
            var service = new ContainerBuilder();
            service.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).AsImplementedInterfaces();

            service.RegisterType<FontSettings>()
                .WithProperty(new TypedParameter(typeof(string), "Times New Roman"))
                .WithProperty(new TypedParameter(typeof(int), 20))
                .InstancePerLifetimeScope();

            service.RegisterType<PathSettings>()
                .WithProperty(new TypedParameter(typeof(string),
                    Path.Join(Directory.GetCurrentDirectory(), "Texts", "SourceText2.txt")))
                .WithProperty(new TypedParameter(typeof(string),
                    Path.Join(Directory.GetCurrentDirectory(), "Texts", "BoringWords.txt")))
                .WithProperty(new TypedParameter(typeof(string),
                    Path.Join(Directory.GetCurrentDirectory(), "Texts", "ru_RU.dic")))
                .WithProperty(new TypedParameter(typeof(string),
                    Path.Join(Directory.GetCurrentDirectory(), "Texts", "ru_RU.aff")))
                .InstancePerLifetimeScope();

            service.RegisterType<Palette>().AsSelf().InstancePerLifetimeScope();
            service.RegisterType<ColorAlgorithm>().AsSelf().InstancePerLifetimeScope();
            service.RegisterType<ImageSettings>().AsSelf().InstancePerLifetimeScope();
            service.RegisterType<PictureBoxImageHolder>().As<IImageHolder, PictureBoxImageHolder>()
                .InstancePerLifetimeScope();

            service.RegisterType<SpiralSettings>().AsSelf().InstancePerLifetimeScope();
            service.RegisterType<SpiralFactory>().AsImplementedInterfaces();
            service.RegisterType<CloudLayouterFactory>().AsImplementedInterfaces();
            service.RegisterType<ReaderFactory>().AsImplementedInterfaces();

            service.RegisterType<CloudVisualization>().InstancePerLifetimeScope();
            service.RegisterType<TagsCloudPainter>().InstancePerLifetimeScope();
            service.RegisterType<TagsHelper>().InstancePerLifetimeScope();
            service.RegisterType<TextAnalyzer>().InstancePerLifetimeScope();
            service.RegisterType<HunspellFactory>().InstancePerLifetimeScope();

            service.RegisterType<MainForm>();
            return service.Build();
        }
    }
}