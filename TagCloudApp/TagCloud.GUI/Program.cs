using System;
using System.Windows.Forms;
using Autofac;
using Autofac.Core;
using TagCloud.Core.Layouters;
using TagCloud.Core.Painters;
using TagCloud.Core.Settings.Interfaces;
using TagCloud.Core.Visualizers;
using TagCloud.Core.WordsParsing;
using TagCloud.Core.WordsParsing.WordsProcessing;
using TagCloud.Core.WordsParsing.WordsProcessing.WordsProcessingUtilities;
using TagCloud.Core.WordsParsing.WordsReading;
using TagCloud.GUI.Extensions;
using TagCloud.GUI.Settings;

namespace TagCloud.GUI
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            var builder = new ContainerBuilder();
            ConfigureDependencies(builder);
            var container = builder.Build();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(container.Resolve<MainForm>());
        }

        private static void ConfigureDependencies(ContainerBuilder builder)
        {
            builder.RegisterType<MainForm>().AsSelf();

            builder.RegisterType<Core.TagCloud>().AsSelf();
            builder.RegisterSettings<GuiTagCloudSettings>().As<ITagCloudSettings>();

            builder.RegisterType<TxtWordsReader>().As<IWordsReader>();
            builder.RegisterType<XmlWordsReader>().As<IWordsReader>();
            builder.RegisterType<GeneralWordsReader>().AsSelf();
            builder.RegisterType<LowerCaseUtility>().As<IWordsProcessingUtility>();
            builder.RegisterType<SimpleWordsProcessor>().As<IWordsProcessor>();
            builder.RegisterSettings<GuiTextParsingSettings>().As<ITextParsingSettings>();
            builder.RegisterType<WordsParser>().AsSelf();

            builder.RegisterType<SimpleTagCloudVisualizer>().As<ITagCloudVisualizer>();
            builder.RegisterSettings<GuiVisualizingSettings>().As<IVisualizingSettings>();
            builder.RegisterType<CircularCloudLayouter>().As<ICloudLayouter>();
            builder.RegisterSettings<GuiLayoutingSettings>().As<ILayoutingSettings>();
            builder.RegisterType<OneColorPainter>().As<IPainter>();
            builder.RegisterSettings<GuiPaintingSettings>().As<IPaintingSettings>();
        }
    }
}