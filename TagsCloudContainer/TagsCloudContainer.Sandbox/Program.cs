using Autofac;
using CommandLine;
using System.Drawing;
using TagsCloudContainer.Core.CLI;
using TagsCloudContainer.Core.Drawer;
using TagsCloudContainer.Core.Results;
using TagsCloudContainer.Core.Options;
using TagsCloudContainer.Core.Layouter;
using TagsCloudContainer.Core.TagsClouds;
using TagsCloudContainer.Core.WordsParser;
using TagsCloudContainer.Core.Drawer.Interfaces;
using TagsCloudContainer.Core.Options.Interfaces;
using TagsCloudContainer.Core.Layouter.Interfaces;
using TagsCloudContainer.Core.TagsClouds.Interfaces;
using TagsCloudContainer.Core.WordsParser.Interfaces;


namespace TagsCloudContainer.Sandbox
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ITagsCloud cloud = null;

            MakeProgramOptions(args)
                .Then(BuildProgram)
                .Then(container => cloud = container.Resolve<ITagsCloud>())
                .Then(_ => cloud.CreateTagCloud())
                .Then(_ => cloud.SaveTagCloud())
                .OnFail(Console.WriteLine);
        }

        private static IContainer BuildProgram(ITagCloudOptions programOptions)
        {
            var builder = new ContainerBuilder();
            builder.RegisterInstance(programOptions.ImageOptions).As<IImageOptions>();
            builder.RegisterInstance(programOptions.FontOptions).As<FontOptions>();
            builder.RegisterInstance(programOptions.FilterOptions).As<IFilterOptions>();
            builder.RegisterType<WordsAnalyzer>().As<IWordsAnalyzer>();
            builder.RegisterType<WordsReader>().As<IWordsReader>().WithParameter("filePath", programOptions.FilePath);
            builder.RegisterType<WordsFilter>().As<IWordsFilter>();
            builder.RegisterType<CircularCloudLayouter>().As<ILayouter>()
                .WithParameter("center",
                    new Point(programOptions.ImageOptions.Width / 2, programOptions.ImageOptions.Height / 2));
            builder.RegisterType<LayoutDrawer>().As<ILayoutDrawer>();
            builder.RegisterType<RectangleLayout>().As<IRectangleLayout>();
            builder.RegisterType<TagsCloud>().As<ITagsCloud>();
            return builder.Build();
        }

        private static Result<TagCloudOptions> MakeProgramOptions(IEnumerable<string> args)
        {
            var options = Parser.Default.ParseArguments<CommandLineOptions>(args).Value;
            if (options == null)
                Result.Fail<TagCloudOptions>("");
            var imageOptions = TagsCloudOptionsValidator.ValidateImageOptions(options);
            var fontOptions = TagsCloudOptionsValidator.ValidateFontOptions(options);
            var filterOptions = TagsCloudOptionsValidator.ValidateFilterOptions(options);

            return options is null
                ? Result.Fail<TagCloudOptions>("")
                : imageOptions
                    .Then(_ => fontOptions)
                    .Then(_ => filterOptions)
                    .Then(_ => new TagCloudOptions(imageOptions.Value, fontOptions.Value, filterOptions.Value,
                        options.FilePath));
        }
    }
}