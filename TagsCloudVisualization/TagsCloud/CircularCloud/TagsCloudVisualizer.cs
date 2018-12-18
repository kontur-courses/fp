using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TagsCloudVisualization.InterfacesForSettings;

namespace TagsCloudVisualization.TagsCloud.CircularCloud
{
    public class TagsCloudVisualizer
    {
        private readonly ITagsCloudSettings cloudSettings;
        private readonly CircularCloudLayouter circularCloudLayouter;
        private readonly double heightStretchFactor;
        private readonly double widthStretchFactor;
        public TagsCloudVisualizer(ITagsCloudSettings cloudSettings)
        {
            heightStretchFactor = 1.8;
            widthStretchFactor = 1.2;
            this.cloudSettings = cloudSettings;
            circularCloudLayouter =
                new CircularCloudLayouter(cloudSettings.ImageSettings.Center, cloudSettings.ImageSettings.ImageSize);

        }
        public Result<Bitmap> DrawCircularCloud()
        {
            return new Result<None>()
                .CheckCondition(() => cloudSettings.WordsSettings.PathToFile == null, "File not found.")
                .CheckCondition(CheckCenterCoordinates, "Image settings are incorrect.")
                .Then(() => cloudSettings.WordsSettings.WordAnalyzer.MakeWordFrequencyDictionary())
                .Then(ProcessWords, "Tag cloud does not fit the image of the specified size.")
                .Then(MakeImage);
        }

        private Bitmap MakeImage(IEnumerable<(Rectangle Region, string word, int RepetitionsCount)> processedWords)
        {
            var imageSettings = cloudSettings.ImageSettings;
            var bmp = new Bitmap(imageSettings.ImageSize.Width, imageSettings.ImageSize.Height);
            var graphics = Graphics.FromImage(bmp);
            var wordsColor = cloudSettings.Palette.WordsColor;
            var font = imageSettings.Font;
            const TextFormatFlags flags = TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter |
                                          TextFormatFlags.NoClipping;

            graphics.Clear(cloudSettings.Palette.BackgroundColor);
            foreach (var processedWord in processedWords)
            {
                var enlargedFont = new Font(font.Name, font.Size * processedWord.RepetitionsCount,
                    font.Style, font.Unit, font.GdiCharSet, font.GdiVerticalFont);
                TextRenderer.DrawText(graphics, processedWord.word, enlargedFont, processedWord.Region, wordsColor, flags);
            }
            return bmp;
        }

        private IEnumerable<(Rectangle Region, string word, int RepetitionsCount)>
            ProcessWords(Dictionary<string, int> frequenciesByWords)
        {
            var imageSettings = cloudSettings.ImageSettings;
            if (circularCloudLayouter.Rectangles.Count != 0)
                circularCloudLayouter.RefreshCircularCloudLayouter(imageSettings.Center, imageSettings.ImageSize);

            var tuples = frequenciesByWords.OrderByDescending(pair => pair.Key.Length)
                .OrderByDescending(pair => pair.Value)
                .Select(pair => (Word: pair.Key, Frequency: pair.Value));
            foreach (var tuple in tuples)
            {
                var nextRegion = GetRectangle(tuple);
                if (circularCloudLayouter.CheckPositionRectangle(nextRegion))
                    yield break;
                yield return (nextRegion, tuple.Word, tuple.Frequency);
            }
        }

        private Rectangle GetRectangle((string Word, int Frequency) tuple)
        {
            var fontSize = cloudSettings.ImageSettings.Font.Size;

            return circularCloudLayouter.PutNextRectangle
                                (new Size((int)(tuple.Word.Length * fontSize * tuple.Frequency * widthStretchFactor),
                                (int)(fontSize * heightStretchFactor * tuple.Frequency)));
        }

        private bool CheckCenterCoordinates()
        {
            var center = cloudSettings.ImageSettings.Center;
            var imageSize = cloudSettings.ImageSettings.ImageSize;
            return center.X < 0 || center.Y < 0 || center.X > imageSize.Width || center.Y > imageSize.Height;
        }
    }
}