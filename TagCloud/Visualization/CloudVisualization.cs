using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ErrorHandling;
using TagCloud.CloudLayouter;
using TagCloud.Extensions;
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

        public Result<Bitmap> Visualize()
        {
            var bitmap = new Bitmap(imageSettings.ImageSize.Width, imageSettings.ImageSize.Height);
            using (var graphics =
                Graphics.FromImage(bitmap))
            {
                SetUpCanvas(graphics);
                SetCenterPoint(graphics);
                return PaintAllWords(graphics)
                    .SelectMany(x => Result.Ok(bitmap))
                    .ReplaceError(x => x + "\nFailed to visualize. Too many words.");
            }
        }

        private void SetUpCanvas(Graphics graphics)
        {
            layouter.ResetLayouter();
            graphics.FillRectangle(new SolidBrush(viewSettings.BackgroundColor), 0, 0,
                imageSettings.ImageSize.Width, imageSettings.ImageSize.Height);
        }

        private Result<None> PaintAllWords(Graphics graphics)
        {
            return words
                .OrderByDescending(w => w.Value)
                .Take(viewSettings.WordsCount)
                .Select(x => PaintRectangleOnCanvas(x.Key, x.Value, graphics))
                .ToOneResult();
        }

        private void SetCenterPoint(Graphics graphics)
        {
            var centerPoint = GetCenterPoint();
            graphics.TranslateTransform(centerPoint.X, centerPoint.Y);
        }

        private Result<None> PaintRectangleOnCanvas(string word, int frequency, Graphics graphics)
        {
            var size = SetNewRectangleSize(frequency, word.Length);
            return layouter.PutNextRectangle(size)
                .Then(rect => TryPaintWordRectangle(graphics, rect))
                .Then(rect => TryPaintWord(word, frequency, graphics, rect));
        }

        private void TryPaintWord(string word, int frequency, Graphics graphics, Rectangle rectangleResult)
        {
            if (!rectangleResult.IsEmpty)
                graphics.DrawString(word,
                    new Font(viewSettings.FontName, GetFontSize(word, rectangleResult.Width)),
                    textColoration.GetTextColor(word, frequency), rectangleResult);
        }

        private Rectangle TryPaintWordRectangle(Graphics graphics, Rectangle rectangle)
        {
            var color = viewSettings.Colors.ElementAt(random.Next(0, viewSettings.Colors.Count));
            if (viewSettings.EnableWordRectangles)
                graphics.FillRectangle(color, rectangle);
            return rectangle;
        }

        private Point GetCenterPoint()
        {
            return new Point(imageSettings.ImageSize.Width / 2, imageSettings.ImageSize.Height / 2);
        }

        private float GetFontSize(string word, int width)
        {
            return width / word.Length;
        }

        private Size SetNewRectangleSize(int frequency, int wordLength)
        {
            var width = (int) ((Math.Log(frequency) + 1) * 10 * wordLength);
            var height = 2 * width / wordLength;
            return new Size(width, height);
        }

        public void ResetWordsFrequenciesDictionary()
        {
            words = frequencyDictionaryMaker.MakeFrequencyDictionary(textConverter.ConvertWords());
        }
    }
}