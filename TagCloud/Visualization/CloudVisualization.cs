using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ErrorHandling;
using TagCloud.CloudLayouter;
using TagCloud.TextColoration;
using TagCloud.TextConversion;

namespace TagCloud.Visualization
{
    public class CloudVisualization
    {
        private readonly ImageSettings imageSettings;
        private readonly ICloudLayouter layouter;
        private readonly ViewSettings viewSettings;
        private readonly ITextColoration textColoration;
        private readonly FrequencyDictionaryMaker frequencyDictionaryMaker;
        private readonly TextConverter textConverter;
        private Dictionary<string, int> words;
        private readonly Random random = new Random();

        public CloudVisualization(ImageSettings imageSettings, ICloudLayouter layouter,
            ViewSettings viewSettings, ITextColoration textColoration,
            FrequencyDictionaryMaker frequencyDictionaryMaker, TextConverter textConverter)
        {
            this.imageSettings = imageSettings;
            this.layouter = layouter;
            this.viewSettings = viewSettings;
            this.textColoration = textColoration;
            this.frequencyDictionaryMaker = frequencyDictionaryMaker;
            this.textConverter = textConverter;
            ResetWordsFrequenciesDictionary();
        }

        public Bitmap Visualize()
        {
            var bitmap = new Bitmap(imageSettings.ImageSize.Width, imageSettings.ImageSize.Height);
            using (var graphics =
                Graphics.FromImage(bitmap))
            {
                var result =
                    SetUpCanvas(graphics)
                        .Then(_ => SetCenterPoint(graphics))
                        .Then(_ => PaintAllWords(graphics));
                result.OnFail(_ => throw new Exception(result.Error));
            }

            return bitmap;
        }

        private Result<None> SetUpCanvas(Graphics graphics)
        {
            layouter.ResetLayouter();
            graphics.FillRectangle(new SolidBrush(viewSettings.BackgroundColor), 0, 0,
                imageSettings.ImageSize.Width, imageSettings.ImageSize.Height);
            return Result.Ok<None>(null);
        }

        private Result<None> PaintAllWords(Graphics graphics)
        {
            foreach (var word in words.OrderByDescending(w => w.Value).Take(viewSettings.WordsCount))
            {
                var result = PaintRectangleOnCanvas(word.Key, word.Value, graphics);
                if (!result.IsSuccess)
                    return Result.Fail<None>(result.Error);
            }

            return Result.Ok<None>(null);
        }

        private Result<None> SetCenterPoint(Graphics graphics)
        {
            var centerPoint = GetCenterPoint();
            graphics.TranslateTransform(centerPoint.X, centerPoint.Y);
            return Result.Ok<None>(null);
        }

        private Result<None> PaintRectangleOnCanvas(string word, int frequency, Graphics graphics)
        {
            var newRectangleSize = SetNewRectangleSize(frequency, word.Length);
            var rectangleResult = layouter.PutNextRectangle(newRectangleSize);

            if (CheckRectangleErrors(rectangleResult, out var paintRectangleOnCanvas)) return paintRectangleOnCanvas;
            TryPaintWordRectangle(graphics, rectangleResult);
            TryPaintWord(word, frequency, graphics, rectangleResult, newRectangleSize);

            return Result.Ok<None>(null);
        }

        private static bool CheckRectangleErrors(Result<Rectangle> rectangleResult,
            out Result<None> paintRectangleOnCanvas)
        {
            paintRectangleOnCanvas = Result.Ok<None>(null);
            if (rectangleResult.IsSuccess) return false;
            paintRectangleOnCanvas = rectangleResult.Error.Equals("Incorrect rectangle position")
                ? Result.Ok<None>(null)
                : Result.Fail<None>("Failed to put next rectangle");
            return true;
        }

        private void TryPaintWord(string word, int frequency, Graphics graphics, Result<Rectangle> rectangleResult,
            Size newRectangleSize)
        {
            if (!rectangleResult.GetValueOrThrow().IsEmpty)
                graphics.DrawString(word, new Font(viewSettings.FontName, GetFontSize(word, newRectangleSize.Width)),
                    textColoration.GetTextColor(word, frequency), rectangleResult.GetValueOrThrow());
        }

        private void TryPaintWordRectangle(Graphics graphics, Result<Rectangle> rectangleResult)
        {
            var color = viewSettings.Colors.ElementAt(random.Next(0, viewSettings.Colors.Count));
            if (viewSettings.EnableWordRectangles)
                graphics.FillRectangle(color, rectangleResult.GetValueOrThrow());
        }

        private Point GetCenterPoint()
            => new Point(imageSettings.ImageSize.Width / 2, imageSettings.ImageSize.Height / 2);

        private float GetFontSize(string word, int width) => width / word.Length;

        private Size SetNewRectangleSize(int frequency, int wordLength)
        {
            var width = (int) ((Math.Log(frequency) + 1) * 10 * wordLength);
            var height = 2 * width / wordLength;
            return new Size(width, height);
        }

        public void ResetWordsFrequenciesDictionary()
            => words = frequencyDictionaryMaker.MakeFrequencyDictionary(textConverter.ConvertWords());
    }
}