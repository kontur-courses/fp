using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Autofac;
using TagsCloudContainer.TagsCloudVisualization;
using TagsCloudContainer.TextPreparation;

namespace TagsCloudContainer.TagsCloudWithWordsVisualization
{
    public static class ConsoleApplication
    {
        public static void Run()
        {
            var container = BuildContainer();
            using var scope = container.BeginLifetimeScope();
            while (true)
            {
                var answer = Console.ReadLine();
                Console.WriteLine(answer.Equals("y"));
                ProcessDialogWithUser(scope,
                    answer.Equals("y")
                        ? GetDefaultSettings(scope, "../../defaultSettings.txt")
                        : GetParametersToVisualize(scope));
            }
        }

        private static void ProcessDialogWithUser(ILifetimeScope scope, (List<string> wordsToVisualize, CircularCloudLayouter layouter, double reductionCoefficient,
            VisualizationParameters parameters, string format) parameters)
        {
            var rnd = new Random();
            var fileName = rnd.Next().ToString();
            var filePath = "Samples/" + fileName + "." + parameters.format;
            Visualizer.GetCloudVisualization(parameters.wordsToVisualize, parameters.layouter,
                    parameters.reductionCoefficient, parameters.parameters)
                .ThenDo(bitmap => FileSaver.Save(bitmap, "../../" + filePath)
                    .GetValueOrThrow());

            Console.WriteLine(scope.ResolveNamed<string>("finalMessage") + filePath);    
        }

        private static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            builder.Register(_ => new TxtFileReader(new WordsReader())).Named<TxtFileReader>("txtReader");
            builder.Register(_ => new DocFileReader(new WordsReader())).Named<DocFileReader>("docReader");
            builder.RegisterInstance(new DefaultWordHelper()).As<IWordsHelper>();
            builder.Register(_ => "Print path to file with words").Named<string>("inputFileMessage");
            builder.Register(_ => "Print colors of tags separated by whitespace").Named<string>("tagColorsMessage");
            builder.Register(_ => "Print background color").Named<string>("backgroundColorMessage");
            builder.Register(_ => new Size(30, 30)).Named<Size>("minTagSize");
            builder.Register(_ => new Size(200, 50)).Named<Size>("maxTagSize");
            builder.Register(_ => "Print coordinates of center of your image separated by whitespace")
                .Named<string>("coordinatesMessage");
            builder.Register(_ => "If you want to use default points generator, print 'y'")
                .Named<string>("useDefaultGeneratorMessage");
            builder.Register(_ => "y").Named<string>("positiveAnswer");
            builder.Register(_ => "Print start spiral radius").Named<string>("radiusMessage");
            builder.Register(_ => "Print start angle").Named<string>("angleMessage");
            builder.Register(_ => "Print angle delta").Named<string>("angleDeltaMessage");
            builder.Register(_ => "Print radius delta").Named<string>("radiusDeltaMessage");
            builder.Register(_ => 0.8).Named<double>("reductionCoefficient");
            builder.Register(_ => 5f).Named<float>("minFontSize");
            builder.Register(_ => FontFamily.GenericSansSerif).Named<FontFamily>("fontFamily");
            builder.Register(_ => "Print text colors separated by whitespace").Named<string>("brushesMessage");
            builder.Register(_ => "Print format to save").Named<string>("formatMessage");
            builder.Register(_ => "Result saved to ").Named<string>("finalMessage");
            return builder.Build();
        }

        private static (List<string> wordsToVisualize, CircularCloudLayouter layouter, double reductionCoefficient,
            VisualizationParameters parameters, string format) GetParametersToVisualize(ILifetimeScope scope)
        {
            var path = AskUserForPath(scope);
            var wordsToVisualize = GetWordsToVisualizeByPath(scope, path);
            var colors = AskUserForTagColors(scope);
            var backgroundColor = AskUserForBackgroundColor(scope);
            var brushes = AskUserForBrushColors(scope);
            var minTagSize = scope.ResolveNamed<Size>("minTagSize");
            var maxTagSize = scope.ResolveNamed<Size>("maxTagSize");
            var center = AskUserForCenterOfImage(scope).GetValueOrThrow();
            var pointsGenerator = AskUserForUsingDefaultGenerator(scope)
                ? AskUserForCustomPointsGenerator(scope, center)
                : new SpiralPointsGenerator(center);
            var reductionCoefficient = scope.ResolveNamed<double>("reductionCoefficient");
            var minFontSize = scope.ResolveNamed<float>("minFontSize");
            var fontFamily = scope.ResolveNamed<FontFamily>("fontFamily");
            var format = AskUserForSavingFormat(scope);
            return (wordsToVisualize, new CircularCloudLayouter(pointsGenerator), reductionCoefficient,
                new VisualizationParameters(colors, backgroundColor, new SizeRange(minTagSize, maxTagSize), fontFamily,
                    minFontSize, brushes), format);
        }

        private static List<string> GetWordsToVisualizeByPath(ILifetimeScope scope, string path)
        {
            var wordsHelper = scope.Resolve<IWordsHelper>();
            var fileReader = GetFileReaderByFilePath(scope, path).GetValueOrThrow();
            var fileWords = fileReader.GetAllWords(path).GetValueOrThrow();
            return  wordsHelper.GetAllWordsToVisualize(fileWords).GetValueOrThrow();    
        }

        private static string AskUserForPath(ILifetimeScope scope)
        {
            Console.WriteLine(scope.ResolveNamed<string>("inputFileMessage"));
            return Console.ReadLine();
        }

        private static List<Color> AskUserForTagColors(ILifetimeScope scope)
        {
            Console.WriteLine(scope.ResolveNamed<string>("tagColorsMessage"));
            return ParseColors(Console.ReadLine());
        }

        private static List<Color> ParseColors(string input)
        {
            return input.Split(' ').Select(Color.FromName).ToList();    
        }

        private static Color ParseColor(string input)
        {
            return Color.FromName(input ?? string.Empty);
        }

        private static Color AskUserForBackgroundColor(ILifetimeScope scope)
        {
            Console.WriteLine(scope.ResolveNamed<string>("backgroundColorMessage"));
            return ParseColor(Console.ReadLine());
        }

        private static List<Brush> AskUserForBrushColors(ILifetimeScope scope)
        {
            Console.WriteLine(scope.ResolveNamed<string>("brushesMessage"));
            return ParseBrushes(Console.ReadLine());
        }

        private static List<Brush> ParseBrushes(string input)
        {
            return ParseColors(input)
                .Select(color => (Brush) new SolidBrush(color))
                .ToList();    
        }

        private static Result<Point> AskUserForCenterOfImage(ILifetimeScope scope)
        {
            Console.WriteLine(scope.ResolveNamed<string>("coordinatesMessage"));
            return ParsePoint(Console.ReadLine());
        }

        private static Result<Point> ParsePoint(string input)
        {
            var coordinates = input.Split(' ').Select(int.Parse).ToList();
            return coordinates.Count() != 2 ? Result.Fail<Point>("Point must contain only 2 coordinates and can't be null") : Result.Ok(new Point(coordinates[0], coordinates[1]));
        }

        private static bool AskUserForUsingDefaultGenerator(ILifetimeScope scope)
        {
            Console.WriteLine(scope.ResolveNamed<string>("useDefaultGeneratorMessage"));
            var answer = Console.ReadLine();
            return answer != null && !answer.Equals(scope.ResolveNamed<string>("positiveAnswer"));
        }

        private static string AskUserForSavingFormat(ILifetimeScope scope)
        {
            Console.WriteLine(scope.ResolveNamed<string>("formatMessage"));
            return Console.ReadLine();
        }

        private static SpiralPointsGenerator AskUserForCustomPointsGenerator(ILifetimeScope scope, Point center)
        {
            Console.WriteLine(scope.ResolveNamed<string>("radiusMessage"));
            var startRadius = double.Parse(Console.ReadLine() ?? string.Empty);
            Console.WriteLine(scope.ResolveNamed<string>("angleMessage"));
            var startAngle = double.Parse(Console.ReadLine() ?? string.Empty);
            Console.WriteLine(scope.ResolveNamed<string>("angleDeltaMessage"));
            var angleDelta = double.Parse(Console.ReadLine() ?? string.Empty);
            Console.WriteLine(scope.ResolveNamed<string>("radiusDeltaMessage"));
            var radiusDelta = double.Parse(Console.ReadLine() ?? string.Empty);
            return new SpiralPointsGenerator(center, startRadius, startAngle, angleDelta, radiusDelta);
        }

        private static Result<IFileReader> GetFileReaderByFilePath(ILifetimeScope scope, string path)
        {
            var format = Path.GetExtension(path);
            if (format.Equals(".txt"))
            {
                return Result.Ok((IFileReader) scope.ResolveNamed<TxtFileReader>("txtReader"));
            }

            if (format.Equals(".docx"))
            {
                return Result.Ok((IFileReader) scope.ResolveNamed<DocFileReader>("docReader"));
            }

            return Result.Fail<IFileReader>("Invalid file extension");
        }

        private static (List<string> wordsToVisualize, CircularCloudLayouter layouter, double reductionCoefficient,
            VisualizationParameters parameters, string format) GetDefaultSettings(ILifetimeScope scope, string settingsPath)
        {
            using var reader = new StreamReader(settingsPath);  
            var lines = reader.ReadToEnd().Split(new[] {Environment.NewLine}, StringSplitOptions.None);
            var words = GetWordsToVisualizeByPath(scope, lines[0]);
            var reductionCoefficient = scope.ResolveNamed<double>("reductionCoefficient");
            var tagColors = ParseColors(lines[1]);
            var backgroundColor = ParseColor(lines[2]);
            var brushes = ParseBrushes(lines[3]);
            var center = ParsePoint(lines[4]).GetValueOrThrow();
            var format = lines[5];

            var parameters = new VisualizationParameters(tagColors, backgroundColor,
                new SizeRange(scope.ResolveNamed<Size>("minTagSize"), scope.ResolveNamed<Size>("maxTagSize")),
                scope.ResolveNamed<FontFamily>("fontFamily"), scope.ResolveNamed<float>("minFontSize"), brushes);
            return (words, new CircularCloudLayouter(new SpiralPointsGenerator(center)),
                scope.ResolveNamed<double>("reductionCoefficient"), parameters, format);
        }
    }
}