using System.Drawing;
using TagsCloudVisualization.ImageRendering;
using System;
using System.Collections.Generic;
using System.Linq;
using TagsCloudVisualization.CloudLayouter;
using TagsCloudVisualization.FontSettings;
using TagsCloudVisualization.Frequency;

namespace TagsCloudVisualization.Visualizer
{
    public class CloudVisualizer : IImageVisualizer
    {
        public Point Center { get; }
        private Bitmap image;
        private readonly Dictionary<string, int> wordsFrequency;
        private Graphics gr;
        private readonly ICloudLayouter layouter;
        private readonly FontFamily fontFamily;
        private readonly string fontColor;
        private readonly IImageRenderingSettings imageRenderingSettings;
        public CloudVisualizer(
            ICloudLayouter layouter,
            IFrequencyCounter frequencyCounter,
            IImageRenderingSettings ImageSettings,
            IFontSettings fontSettings)
        {
            Center = layouter.Spiral.Center;
            wordsFrequency = frequencyCounter.GetFrequency().Value;
            this.layouter = layouter;
            fontFamily = fontSettings.FontFamily;
            fontColor = fontSettings.FontColor;
            imageRenderingSettings = ImageSettings;
        }

        private Result<Bitmap> CreateBitmap()
        {
            if (imageRenderingSettings.Width <= 0 || imageRenderingSettings.Height <= 0)
            {
                return Result.Fail<Bitmap>("Please correct the image dimensions. They cannot be non-positive.");
            }

            return new Bitmap(imageRenderingSettings.Width, imageRenderingSettings.Height);
        }

        public Result<Image> CreateImage()
        {
            var createBitmapResult = CreateBitmap();

            if (!createBitmapResult.IsSuccess)
                return Result.Fail<Image>(createBitmapResult.Error);

            image = createBitmapResult.Value;
            gr = Graphics.FromImage(image);

            var fillImageResult = Result.OfAction(() => gr.FillRectangle(
                new SolidBrush(Color.White), new Rectangle(0, 0, image.Width, image.Height)));

            if (!fillImageResult.IsSuccess)
                return Result.Fail<Image>(fillImageResult.Error);

            var drawWordsResult = DrawWords();

            if (!drawWordsResult.IsSuccess)
                return Result.Fail<Image>(drawWordsResult.Error);

            return Result.AsResult((Image)image);
        }

        private Result<None> DrawWords()
        {
            var brush = new SolidBrush(GetColor().Value);
            var getTagsResult = GetTags();
            
            if (!getTagsResult.IsSuccess)
                return Result.Fail<None>(getTagsResult.RefineError("Please check the data source and its path").Error);

            var tags = getTagsResult.Value;

            var stringFormat = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center,
            };

            var checkCloudSizeResult = CheckCloudSize(tags);

            if (!checkCloudSizeResult.IsSuccess)
                return Result.Fail<None>(checkCloudSizeResult.Error);

            if (!checkCloudSizeResult.Value)
                return Result.Fail<None>("The cloud does not fit into the size of the image. Reduce the number of original words");

            foreach (var tag in tags)
            {
                if (fontColor == "random")
                    brush.Color = GetRandomColor();

                gr.DrawString(tag.word, tag.font, brush, tag.location, stringFormat);
            }

            return Result.Ok();
        }

        private Result<List<Tag>> GetTags()
        {
            var minFrequencyResult = Result.Of( () => wordsFrequency.Min(x => x.Value));
            var maxFrequencyResult = Result.Of(() => wordsFrequency.Max(x => x.Value));

            if (!minFrequencyResult.IsSuccess || !maxFrequencyResult.IsSuccess)
                return Result.Fail<List<Tag>>(minFrequencyResult.Error);

            var tags = new List<Tag>();

            foreach (var wordFreqPair in wordsFrequency.OrderByDescending(x => x.Value))
            {
                var font = GetFont(12, 50, minFrequencyResult.Value, 
                    maxFrequencyResult.Value, wordFreqPair.Value).Value;

                var tagsAddingResult = Result.Of(() => GetWordSize(wordFreqPair.Key, font)).Value
                    .Then(rectSize => layouter.PutNextRectangle(rectSize))
                    .Then((rect) => tags.Add(new Tag(wordFreqPair.Key, font, rect)));

                if (!tagsAddingResult.IsSuccess)
                    return Result.Fail<List<Tag>>(tagsAddingResult.Error);
            }

            return tags;
        }

        private Result<bool> CheckCloudSize(IEnumerable<Tag> tags)
        {
            var listOfTags = tags.ToList();

            var maxX = listOfTags.Max(tag => tag.location.X);
            var minX = listOfTags.Min(tag => tag.location.X);
            var maxY = listOfTags.Max(tag => tag.location.Y);
            var minY = listOfTags.Min(tag => tag.location.Y);

            return minX >= 0 && minY >= 0 && maxX <= image.Size.Width && maxY <= image.Size.Height;
        }

        private Result<Color> GetColor()
        {
            if (fontColor == "random")
                return GetRandomColor();

            return ParseColor();
        }

        private Result<Color> ParseColor()
        {
            var separators = new[] { ',', '.', ' ' };
            var argbResult = Result.Of(() => fontColor.Split(separators).Select(x => int.Parse(x)).ToArray());

            if (argbResult.IsSuccess)
                return Color.FromArgb(argbResult.Value[0], argbResult.Value[1], argbResult.Value[2], argbResult.Value[3]);

            return Result.Fail<Color>(argbResult.Error);
        }

        private Result<Font> GetFont(int minSize, int maxSize, double minFrequency, double maxFrequency, double wordFrequency)
        {
            var fontSize = (int)(minSize + (maxSize - minSize) * (wordFrequency - minFrequency) / (maxFrequency - minFrequency));

            var createFontResult = Result.Of(() => new Font(fontFamily, fontSize));

            if (createFontResult.IsSuccess)
                return createFontResult;

            return Result.Fail<Font>(createFontResult.Error);
        }

        private static Color GetRandomColor()
        {
            var random = new Random();
            return Color.FromArgb(
                (byte)random.Next(0, 255),
                (byte)random.Next(0, 255),
                (byte)random.Next(0, 255));
        }

        private Result<Size> GetWordSize(string word, Font font)
        {
            var wordSizeResult = Result.Of(() => gr.MeasureString(word, font));

            if (!wordSizeResult.IsSuccess)
                return Result.Fail<Size>(wordSizeResult.Error);

            var width = (int)Math.Ceiling(wordSizeResult.Value.Width);
            var height = (int)Math.Ceiling(wordSizeResult.Value.Height);

            return new Size(width, height);
        }

        private struct Tag
        {
            public string word;
            public Font font;
            public Rectangle location;

            public Tag(string word, Font font, Rectangle location)
            {
                this.word = word;
                this.font = font;
                this.location = location;
            }
        }
    }
}