using System;
using Autofac;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using TagsCloudVisualizationDI.AnalyzedTextReader;
using TagsCloudVisualizationDI.Layouter.Filler;
using TagsCloudVisualizationDI.Saving;
using TagsCloudVisualizationDI.TextAnalyze;
using TagsCloudVisualizationDI.TextAnalyze.Analyzer;
using TagsCloudVisualizationDI.TextAnalyze.Normalizer;
using TagsCloudVisualizationDI.TextAnalyze.Visualization;

namespace TagsCloudVisualizationDI
{
    public class Program
    {
        private const string Arguments = "-lndw -ig";
        private const int Multiplier = 25;
        private static readonly Point Center = new Point(2500, 2500);
        private static readonly SolidBrush Brush = new SolidBrush(Color.Black);
        private static readonly Font Font = new Font("Times", 15);
        private static readonly Size ImageSize = new Size(5000, 5000);
        private static readonly Encoding Encoding = Encoding.UTF8;
        private static readonly Size ElementSize = new Size(100, 100);

        private static readonly string MyStemPath = Path.GetDirectoryName(typeof(Program).Assembly.Location) + "\\mystem.exe";
        private static readonly string SaveAnalizationPath = Path.GetDirectoryName(typeof(Program).Assembly.Location) + "\\result.TXT";


        public static void Main(string pathToFile, string pathToSave, 
            Result<ImageFormat> imageFormat, Result<List<string>> excludedWordsList)
        {
            var excludedSpeechParts = new[]
            {
                PartsOfSpeech.SpeechPart.CONJ, PartsOfSpeech.SpeechPart.INTJ,
                PartsOfSpeech.SpeechPart.PART, PartsOfSpeech.SpeechPart.PR,
            };

            imageFormat.OnFail(error => PrintAboutFail(error));
            excludedWordsList.OnFail(error => PrintAboutFail(error));


            Checker.CheckPathToFile(pathToFile);
            Checker.CheckPathToDirectory(pathToSave
                .Substring(0, pathToSave.LastIndexOf("\\", StringComparison.InvariantCulture)+1));
            Checker.CheckPathToFile(MyStemPath);
            Checker.CheckPathToFile(SaveAnalizationPath);


            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<DefaultNormalizer>().As<INormalizer>();

            containerBuilder.RegisterType<DefaultSaver>().As<ISaver>()
                .WithParameter("savePath", pathToSave)
                .WithParameter("imageFormat", imageFormat.GetValueOrThrow() ?? ImageFormat.Png);


            containerBuilder.RegisterType<DefaultAnalyzer>().As<IAnalyzer>()
                .WithParameter("excludedSpeechParts", excludedSpeechParts)
                .WithParameter("excludedWords", excludedWordsList.GetValueOrThrow() ?? new List<string>())
                .WithParameter("filePath", pathToFile)
                .WithParameter("mystemPath", MyStemPath)
                .WithParameter("arguments", Arguments)
                .WithParameter("saveAnalyzePath", SaveAnalizationPath);

            containerBuilder.RegisterType<CircularCloudLayouterForRectanglesWithText>().As<IContentFiller>()
                .WithParameter("center", Center);


            containerBuilder.RegisterType<DefaultVisualization>().As<IVisualization>()
                .WithParameter("brush", Brush)
                .WithParameter("font", Font)
                .WithParameter("imageSize", ImageSize)
                .WithParameter("sizeMultiplier", Multiplier);

            containerBuilder.RegisterType<DefaultAnalyzedTextFileReader>().As<IAnalyzedTextFileReader>()
                .WithParameter("preAnalyzedTextPath", SaveAnalizationPath)
                .WithParameter("encoding", Encoding);



            var buildContainer = containerBuilder.Build();

            var analyzer = buildContainer.Resolve<IAnalyzer>();
            var normalizer = buildContainer.Resolve<INormalizer>();
            var filler = buildContainer.Resolve<IContentFiller>();
            var reader = buildContainer.Resolve<IAnalyzedTextFileReader>();
            var saver = buildContainer.Resolve<ISaver>();
            var visualization = buildContainer.Resolve<IVisualization>();



            var invokeResult = analyzer.InvokeMystemAnalizationResult();


            if (!invokeResult.IsSuccess)
                PrintAboutFail(invokeResult.Error);

            reader.ReadText()
                .Then(analyzedWords => analyzer.GetAnalyzedWords(analyzedWords))

                .Then(normalizedWords => NormalizeWords(normalizedWords, normalizer))

                .Then(formedElement => 
                    filler.FormStatisticElements(ElementSize, formedElement.ToList()))

                .Then(sizedElement => 
                    visualization.FindSizeForElements(sizedElement)
                        .OrderByDescending(el => el.WordElement.CntOfWords).ToList())

                .Then(positionedElement => filler.MakePositionElements(positionedElement))

                .Then(res => 
                    visualization.DrawAndSaveImage(res, saver.GetSavePath(), imageFormat.GetValueOrThrow())
                    .OnFail(er => PrintAboutFail(er)));
        }

        internal static void PrintAboutFail(string error)
        {
            throw new Exception(error);
        }

        private static IEnumerable<Word> NormalizeWords(IEnumerable<Word> analyzedWords, INormalizer normalizer)
        {
            foreach (var word in analyzedWords)
            {
                word.WordText = normalizer.Normalize(word.WordText);
                yield return word;
            }
        }
    }
}
