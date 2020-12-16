using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Autofac;
using CommandLine;
using NHunspell;
using TagCloud.ConsoleAppHelper;
using TagCloud.Curves;
using TagCloud.ErrorHandling;
using TagCloud.Visualization;
using TagCloud.WordsFilter;
using TagCloud.WordsProvider;

namespace TagCloud
{
    public class Program
    {
        private static Result<IContainer> BuildDependencies(
            int width,
            int height,
            Color[] colors,
            string input)
        {
            var builder = new ContainerBuilder();
            var center = new Point(width / 2, height / 2);
            const double density = 0.05;
            const int angelStep = 5;

            builder.RegisterType<ArchimedeanSpiral>().As<ICurve>()
                .WithParameter("center", center)
                .WithParameter("density", density)
                .WithParameter("angelStep", angelStep);
            builder.RegisterType<CircularCloudLayouter>().As<ITagCloud>().SingleInstance();
            builder.RegisterType<TagCloudVisualizer>().As<IVisualizer>();

            var wordsFilePath = Path.GetFullPath(input);
            var wordsProvider = FileWordsProviderFactory.Create(wordsFilePath);
            if (!wordsProvider.IsSuccess)
                return Result.Fail<IContainer>(wordsProvider.Error);
            builder.RegisterInstance(wordsProvider.Value).As<IWordsProvider>();

            var dictionaryAff = Path.GetFullPath("../../../../dictionaries/en.aff");
            var dictionaryDic = Path.GetFullPath("../../../../dictionaries/en.dic");
            var hunspell = Result.Of(() => new Hunspell(dictionaryAff, dictionaryDic))
                .RefineError("Hunspell was unable to find dictionaries");
            if (!hunspell.IsSuccess)
                return Result.Fail<IContainer>(hunspell.Error);

            var wordsFilter = new WordsFilter.WordsFilter()
                .Normalize(hunspell.Value)
                .RemovePrepositions();
            builder.RegisterInstance(wordsFilter).As<IWordsFilter>();

            builder.RegisterInstance(colors).As<Color[]>();
            return Result.Of(() => builder.Build());
        }

        private static Result<IContainer> BuildDependencies(Options options)
        {
            var colors = ColorsParser.ParseColors(options.Colors);
            if (!colors.IsSuccess)
                return Result.Fail<IContainer>(colors.Error);
            return BuildDependencies(options.Width, options.Height, colors.Value, options.Input);
        }

        public static void Main(string[] args)
        {
            var result = Parser.Default.ParseArguments<Options>(args);
            var options = Result.Of(() => ((Parsed<Options>) result).Value);
            if (!options.IsSuccess)
            {
                foreach (var error in ((NotParsed<Options>) result).Errors)
                    Console.WriteLine(error);
                return;
            }

            var container = BuildDependencies(options.Value)
                .OnFail(Console.WriteLine);
            if (!container.IsSuccess)
                return;

            var tagCloud = container.Value.Resolve<ITagCloud>().GenerateTagCloud()
                .OnFail(Console.WriteLine);
            if (!tagCloud.IsSuccess)
                return;

            var font = options.Value.FontFamily;
            var colors = container.Value.Resolve<Color[]>();
            var bitmap = container.Value.Resolve<IVisualizer>()
                .CreateBitMap(options.Value.Width, options.Value.Height, colors, font)
                .OnFail(Console.WriteLine);
            if (!bitmap.IsSuccess)
                return;
            bitmap.Value.Save(Path.GetFullPath(options.Value.Output), ImageFormat.Png);
        }
    }
}