using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
                Console.WriteLine(scope.ResolveNamed<string>("defaultSettingsMessage"));
                var cloudParameters = Console.ReadLine()!.Equals(scope.ResolveNamed<string>("positiveAnswer"))
                    ? GetDefaultSettings(scope, "../../defaultSettings.txt")
                    : GetParametersToVisualize(scope);
                ProcessDialogWithUser(scope, cloudParameters);
            }
        }

        private static void ProcessDialogWithUser(ILifetimeScope scope, TagCloudParameters parameters)
        {
            var rnd = new Random();
            var fileName = rnd.Next().ToString();
            var filePath = "Samples/" + fileName + "." + parameters.SaveFormat;
            Visualizer.GetCloudVisualization(parameters)
                .ThenDo(bitmap => FileSaver.Save(bitmap, "../../" + filePath).GetValueOrThrow())
                .ThenDo(_ => Console.WriteLine(scope.ResolveNamed<string>("finalMessage") + filePath))
                .OnFail(error => Console.WriteLine("Error: " + error));
        }

        private static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            builder.Register(_ => new TxtFileReader(new WordsReader())).Named<TxtFileReader>("txtReader");
            builder.Register(_ => new DocFileReader(new WordsReader())).Named<DocFileReader>("docReader");
            builder.RegisterInstance(new DefaultWordHelper()).As<IWordsHelper>();
            builder.Register(_ => "Print 'y' if you want to use default settings")
                .Named<string>("defaultSettingsMessage");
            builder.Register(_ => "Print path to file with words").Named<string>("inputFileMessage");
            builder.Register(_ => "Print colors of tags separated by whitespace").Named<string>("tagColorsMessage");
            builder.Register(_ => "Print background color").Named<string>("backgroundColorMessage");
            builder.Register(_ => new SizeRange(new Size(30, 30), new Size(200, 50))).Named<SizeRange>("sizeRange");
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

        private static (SizeRange sizeRange, double reductionCoefficient, float minFontSize, FontFamily fontFamily)
            GetConstantParameters(ILifetimeScope scope)
        {
            return (scope.ResolveNamed<SizeRange>("sizeRange"), scope.ResolveNamed<double>("reductionCoefficient"),
                scope.ResolveNamed<float>("minFontSize"), scope.ResolveNamed<FontFamily>("fontFamily"));
        }

        private static TagCloudParameters GetParametersToVisualize(ILifetimeScope scope)
        {
            var constants = GetConstantParameters(scope);
            var path = AskUserForPath(scope);
            var wordsToVisualize = GetWordsToVisualizeByPath(scope, path);
            var colors = AskUserForTagColors(scope);
            var backgroundColor = AskUserForBackgroundColor(scope);
            var brushes = AskUserForBrushColors(scope);
            var center = AskUserForCenterOfImage(scope).GetValueOrThrow();
            var pointsGenerator = AskUserForUsingDefaultGenerator(scope)
                ? AskUserForCustomPointsGenerator(scope, center)
                : new SpiralPointsGenerator(center);
            var format = AskUserForSavingFormat(scope);
            return new TagCloudParameters(wordsToVisualize, new CircularCloudLayouter(pointsGenerator),
                constants.reductionCoefficient,
                new VisualizationParameters(colors, backgroundColor, constants.sizeRange, constants.fontFamily,
                    constants.minFontSize, brushes), format);
        }

        private static string AskUserForPath(ILifetimeScope scope)
        {
            Console.WriteLine(scope.ResolveNamed<string>("inputFileMessage"));
            return Console.ReadLine();
        }

        private static List<Color> AskUserForTagColors(ILifetimeScope scope)
        {
            Console.WriteLine(scope.ResolveNamed<string>("tagColorsMessage"));
            return Parser.ParseColors(Console.ReadLine());
        }

        private static Color AskUserForBackgroundColor(ILifetimeScope scope)
        {
            Console.WriteLine(scope.ResolveNamed<string>("backgroundColorMessage"));
            return Parser.ParseColor(Console.ReadLine());
        }

        private static List<Brush> AskUserForBrushColors(ILifetimeScope scope)
        {
            Console.WriteLine(scope.ResolveNamed<string>("brushesMessage"));
            return Parser.ParseBrushes(Console.ReadLine());
        }

        private static Result<Point> AskUserForCenterOfImage(ILifetimeScope scope)
        {
            Console.WriteLine(scope.ResolveNamed<string>("coordinatesMessage"));
            return Parser.ParsePoint(Console.ReadLine());
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

        private static List<string> GetWordsToVisualizeByPath(ILifetimeScope scope, string path)
        {
            var wordsHelper = scope.Resolve<IWordsHelper>();
            var fileReader = GetFileReaderByFilePath(scope, path).GetValueOrThrow();
            var fileWords = fileReader.GetAllWords(path).GetValueOrThrow();
            return wordsHelper.GetAllWordsToVisualize(fileWords).GetValueOrThrow();
        }

        private static Result<IFileReader> GetFileReaderByFilePath(ILifetimeScope scope, string path)
        {
            var format = Path.GetExtension(path);
            return format switch
            {
                ".txt" => Result.Ok((IFileReader) scope.ResolveNamed<TxtFileReader>("txtReader")),
                ".docx" => Result.Ok((IFileReader) scope.ResolveNamed<DocFileReader>("docReader")),
                _ => Result.Fail<IFileReader>("Invalid file extension")
            };
        }

        private static TagCloudParameters GetDefaultSettings(ILifetimeScope scope, string settingsPath)
        {
            using var reader = new StreamReader(settingsPath);
            var constants = GetConstantParameters(scope);
            var lines = reader.ReadToEnd().Split(new[] {Environment.NewLine}, StringSplitOptions.None);
            var words = GetWordsToVisualizeByPath(scope, lines[0]);
            var tagColors = Parser.ParseColors(lines[1]);
            var backgroundColor = Parser.ParseColor(lines[2]);
            var brushes = Parser.ParseBrushes(lines[3]);
            var center = Parser.ParsePoint(lines[4]).GetValueOrThrow();
            var format = lines[5];
            var parameters = new VisualizationParameters(tagColors, backgroundColor, constants.sizeRange,
                constants.fontFamily, constants.minFontSize, brushes);
            return new TagCloudParameters(words, new CircularCloudLayouter(new SpiralPointsGenerator(center)),
                constants.reductionCoefficient, parameters, format);
        }
    }
}