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
using TagsCloudVisualizationDI.Settings;
using TagsCloudVisualizationDI.TextAnalization;
using TagsCloudVisualizationDI.TextAnalization.Analyzer;
using TagsCloudVisualizationDI.TextAnalization.Normalizer;
using TagsCloudVisualizationDI.TextAnalization.Visualization;

namespace TagsCloudVisualizationDI
{
    public class Program
    {
        public static void Main(string pathToFile, string pathToSave, ImageFormat imageFormat, List<string> excludedWordsList)
        {
            var excludedSpeechParts = new[]
            {
                PartsOfSpeech.SpeechPart.CONJ, PartsOfSpeech.SpeechPart.INTJ,
                PartsOfSpeech.SpeechPart.PART, PartsOfSpeech.SpeechPart.PR,
            };
            var myStemPath = Path.GetDirectoryName(typeof(Program).Assembly.Location) + "\\mystem.exe";
            //var myStemPath = "U:\\" + "\\mystAm.exe";
            var saveAnalizationPath = Path.GetDirectoryName(typeof(Program).Assembly.Location) + "\\result.TXT"; //!!!!
            var arguments = "-lndw -ig";
            var center = new Point(2500, 2500);
            var brush = new SolidBrush(Color.Black);
            var textFont = new Font("Times", 15);
            var imageSize = new Size(5000, 5000);
            var multiplier = 25;
            var encoding = Encoding.UTF8;




            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<DefaultNormalizer>().As<INormalizer>();

            containerBuilder.RegisterType<DefaultSaver>().As<ISaver>()
                .WithParameter("savePath", pathToSave)
                .WithParameter("imageFormat", imageFormat ?? ImageFormat.Png);

            
            containerBuilder.RegisterType<DefaultAnalyzer>().As<IAnalyzer>()
                .WithParameter("excludedSpeechParts", excludedSpeechParts)
                .WithParameter("excludedWords", excludedWordsList ?? new List<string>())
                .WithParameter("filePath", pathToFile)
                .WithParameter("saveAnalizationPath", saveAnalizationPath)
                .WithParameter("mystemPath", myStemPath)
                .WithParameter("arguments", arguments);

            containerBuilder.RegisterType<CircularCloudLayouterForRectanglesWithText>().As<IContentFiller>()
                .WithParameter("center", center);


            containerBuilder.RegisterType<DefaultVisualization>().As<IVisualization>()
                .WithParameter("brush", brush)
                .WithParameter("font", textFont)
                .WithParameter("imageSize", imageSize)
                .WithParameter("sizeMultiplier", multiplier);

            containerBuilder.RegisterType<DefaultTextFileReader>().As<ITextFileReader>()
                .WithParameter("preAnalyzedTextPath", saveAnalizationPath)
                .WithParameter("encoding", encoding);



            var buildContainer = containerBuilder.Build();

            var analyzer = buildContainer.Resolve<IAnalyzer>();
            var normalizer = buildContainer.Resolve<INormalizer>();
            var filler = buildContainer.Resolve<IContentFiller>();
            var reader = buildContainer.Resolve<ITextFileReader>();
            var saver = buildContainer.Resolve<ISaver>();
            var elementSize = new Size(100, 100);
            var format = imageFormat;
            var visualization = buildContainer.Resolve<IVisualization>();




            var invokeResult = analyzer.InvokeMystemAnalizationResult();
            if (!invokeResult.IsSuccess)
                PrintAboutFail(invokeResult.Error);



            var wordsFromFile = reader.ReadText().OnFail(er => PrintAboutFail(er));



            List<Word> analyzedWords = analyzer.GetAnalyzedWords(wordsFromFile).ToList();
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
