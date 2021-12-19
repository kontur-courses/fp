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
using TagsCloudVisualizationDI.TextAnalization;
using TagsCloudVisualizationDI.TextAnalization.Analyzer;
using TagsCloudVisualizationDI.TextAnalization.Normalizer;
using TagsCloudVisualizationDI.TextAnalization.Visualization;

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

        private static readonly string MyStemPath = Path.GetDirectoryName(typeof(Program).Assembly.Location) + "\\mystem.exe";
        private static readonly string SaveAnalizationPath = Path.GetDirectoryName(typeof(Program).Assembly.Location) + "\\result.TXT"; //!!!!


        public static void Main(string pathToFile, string pathToSave, Result<ImageFormat> imageFormat, Result<List<string>> excludedWordsList)
        {
            var excludedSpeechParts = new[]
            {
                PartsOfSpeech.SpeechPart.CONJ, PartsOfSpeech.SpeechPart.INTJ,
                PartsOfSpeech.SpeechPart.PART, PartsOfSpeech.SpeechPart.PR,
            };

            imageFormat.OnFail(error => PrintAboutFail(error));
            excludedWordsList.OnFail(error => PrintAboutFail(error));



            Checker.CheckPathToFile(pathToFile);
            Checker.CheckPathToDirectory(pathToSave.Substring(0, pathToSave.LastIndexOf("\\")+1));
            Checker.CheckPathToFile(MyStemPath);
            Checker.CheckPathToFile(SaveAnalizationPath);


            //var arguments = "-lndw -ig";
            //var center = new Point(2500, 2500);
            //var brush = new SolidBrush(Color.Black);
            //var textFont = new Font("Times", 15);
            //var imageSize = new Size(5000, 5000);
            //var multiplier = 25;
            //var encoding = Encoding.UTF8;




            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<DefaultNormalizer>().As<INormalizer>();

            containerBuilder.RegisterType<DefaultSaver>().As<ISaver>()
                .WithParameter("savePath", pathToSave)
                .WithParameter("imageFormat", imageFormat.Value ?? ImageFormat.Png);

            
            containerBuilder.RegisterType<DefaultAnalyzer>().As<IAnalyzer>()
                .WithParameter("excludedSpeechParts", excludedSpeechParts)
                .WithParameter("excludedWords", excludedWordsList.Value ?? new List<string>())
                .WithParameter("filePath", pathToFile)
                .WithParameter("saveAnalizationPath", SaveAnalizationPath)
                .WithParameter("mystemPath", MyStemPath)
                .WithParameter("arguments", Arguments);

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
            var elementSize = new Size(100, 100);
            var format = imageFormat.Value;
            var visualization = buildContainer.Resolve<IVisualization>();




            var invokeResult = analyzer.InvokeMystemAnalizationResult();


            if (!invokeResult.IsSuccess)
                PrintAboutFail(invokeResult.Error);



            var wordsFromFile = reader.ReadText();



            var analyzedWords = analyzer.GetAnalyzedWords(wordsFromFile).ToList();
            var normalyzedWords = NormalyzeWords(analyzedWords, normalizer).ToList();
            var formedElements = filler.FormStatisticElements(elementSize, normalyzedWords);
            var sizedElements = visualization.FindSizeForElements(formedElements);
            var sortedElements = sizedElements.
                OrderByDescending(el => el.WordElement.CntOfWords).ToList();
            var positionedElements = filler.MakePositionElements(sortedElements);

            visualization.DrawAndSaveImage(positionedElements, saver.GetSavePath(), format).OnFail(er => PrintAboutFail(er));
        }

        private static void PrintAboutFail(string error)
        {
            throw new Exception(error);
        }

        private static IEnumerable<Word> NormalyzeWords(IEnumerable<Word> analyzedWords, INormalizer normalizer)
        {
            foreach (var word in analyzedWords)
            {
                word.WordText = normalizer.Normalize(word.WordText);
                yield return word;
            }
        }
    }
}
