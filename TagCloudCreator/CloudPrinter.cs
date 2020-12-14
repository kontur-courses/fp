using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using CloudLayouters;
using ResultOf;

namespace TagCloudCreator
{
    public class CloudPrinter
    {
        private readonly IFileReader[] readers;


        public CloudPrinter(IFileReader[] readers)
        {
            this.readers = readers;
        }

        private static Result<Bitmap> DrawCloud(IEnumerable<DrawingWord> words, Size imageSize,
            IColorSelector colorSelector,
            IWordPainter wordPainter)
        {
            var cloud = new Bitmap(imageSize.Width, imageSize.Height);
            var graphics = Graphics.FromImage(cloud);
            foreach (var word in words)
                wordPainter.DrawWord(word.Word, word.Font,
                    new SolidBrush(colorSelector.GetColor(word)), graphics,
                    word.Location);
            return cloud;
        }

        public Result<Bitmap> DrawCloud(string pathToWordsFile, BaseCloudLayouter layouter, Size imageSize,
            FontFamily fontFamily, IColorSelector colorSelector, IWordPainter? wordPainter = null)
        {
            wordPainter ??= new SimpleWordPainter();
            layouter.ClearLayout();
            if (!File.Exists(pathToWordsFile))
                return Result.Fail<Bitmap>($"File {pathToWordsFile} not found");
            var interestingWords =
                Result.Of(() => Path.GetExtension(pathToWordsFile))
                    .Then(ext => readers.First(x => x.Types.Contains(ext)))
                    .Then(reader => reader.ReadAllLinesFromFile(pathToWordsFile))
                    .Then(WordPreparer.GetInterestingWords);
            var statistic = interestingWords.Then(words => WordPreparer.GetWordsStatistic(words));
            return statistic.Then(_ =>
                DrawCloud(
                    RectanglesForWordsCreator.GetReadyWords(
                        statistic.GetValueOrThrow(),
                        layouter,
                        fontFamily,
                        wordPainter),
                    imageSize,
                    colorSelector,
                    wordPainter));
        }
    }
}