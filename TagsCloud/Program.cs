using System;
using System.Drawing;
using Autofac;
using CommandLine;
using TagsCloud.Drawer;
using TagsCloud.Layouter;
using TagsCloud.ProgramOptions;
using TagsCloud.Result;
using TagsCloud.WordsParser;

namespace TagsCloud
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var options = Parser.Default.ParseArguments<CommandLineOptions>(args).Value;
            var imageOptions = new ImageOptions(options.Width, options.Height, options.OutputDirectory,
                options.OutputFileName, options.OutputFileExtension);
            var fontOptions = new FontOptions(options.FontFamily, options.FontColor);
            var filterOptions = new FilterOptions(options.MystemLocation, options.BoringWords);
            var builder = new ContainerBuilder();

            builder.RegisterInstance(imageOptions).As<IImageOptions>();
            builder.RegisterInstance(fontOptions).As<IFontOptions>();
            builder.RegisterInstance(filterOptions).As<IFilterOptions>();
            builder.RegisterType<WordsAnalyzer>().As<IWordsAnalyzer>();
            builder.RegisterType<FileReader>().As<IWordReader>().WithParameter("filePath", options.FilePath);
            builder.RegisterType<Filter>().As<IFilter>();
            builder.RegisterType<CircularCloudLayouter>().As<ILayouter>()
                .WithParameter("center", new Point(options.Width / 2, options.Height / 2));
            builder.RegisterType<LayoutDrawer>().As<ILayoutDrawer>();
            builder.RegisterType<RectangleLayout>().As<IRectangleLayout>();
            builder.RegisterType<TagCloud>().As<ITagCloud>();
            var container = builder.Build();

            var tagCloudContainer = container.Resolve<ITagCloud>();
            tagCloudContainer.MakeTagCloud()
                .Then(_ => tagCloudContainer.SaveTagCloud())
                .OnFail(Console.WriteLine);
        }
    }
}