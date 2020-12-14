using System;
using System.Windows.Forms;
using Autofac;
using ResultOf;
using TagCloudCreator;

namespace TagCloudGUIClient
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Result.Of(InitializeContainer).Then(x => Application.Run(x.Resolve<Form>()));
        }

        private static IContainer InitializeContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Form1>().AsSelf().As<Form>();
            builder.RegisterType<CircularLayoutFabric>().As<IBaseCloudLayouterFabric>();
            builder.RegisterType<RectangleLayouterFabric>().As<IBaseCloudLayouterFabric>();
            builder.RegisterType<FullRandomColorSelectorFabric>().As<IColorSelectorFabric>();
            builder.RegisterType<RandomFromKnownColorsSelectorFabric>().As<IColorSelectorFabric>();
            builder.RegisterType<BlackColorSelectorFabric>().As<IColorSelectorFabric>();
            builder.RegisterType<CloudPrinter>().AsSelf();
            builder.RegisterType<TxtFileReader>().As<IFileReader>().AsSelf();
            builder.RegisterType<TextExtractorBasedReader>().As<IFileReader>();
            return builder.Build();
        }
    }
}