using System.Drawing;
using System.Reflection;
using TagsCloudVisualization;
using System.Drawing.Imaging;
using TagCloudGenerator.TextReaders;
using TagCloudGenerator.TextProcessors;

namespace TagCloudGenerator
{
    public class TagCloudDrawer
    {
        private ITextProcessor[] textProcessors;
        private ITextReader[] textReaders;
        private WordCounter wordCounter;

        public TagCloudDrawer(WordCounter wordCounter, IEnumerable<ITextProcessor> textProcessors, IEnumerable<ITextReader> textReaders)
        {
            this.textProcessors = textProcessors.ToArray();
            this.textReaders = textReaders.ToArray();
            this.wordCounter = wordCounter;
        }

        public Result<Bitmap> DrawWordsCloud(string filePath, VisualizingSettings visualizingSettings)
        {
            if (filePath == null)
                return Result<Bitmap>.Failure("There is no path to the file");

            var text = ReadText(filePath);            
            if (!text.IsSuccess)
                return Result<Bitmap>.Failure(text.Error);

            var words = text.Value;
            if (words.Count == 0)
                return Result<Bitmap>.Failure(string.Format("The file is empty"));

            words = ProcessText(textProcessors, words);           
            var wordsWithCount = wordCounter.CountWords(words);

            ImageScaler imageScaler = new ImageScaler(wordsWithCount);
            var rectangles = GetRectanglesToDraw(wordsWithCount, visualizingSettings);
            var scaleImage = GetScaleImage(rectangles, imageScaler, visualizingSettings);

            if (scaleImage.IsSuccess)
                return Result<Bitmap>.Success(scaleImage.Value);

            var image = Draw(wordsWithCount, visualizingSettings, rectangles);
            if (image == null)
                return Result<Bitmap>.Failure("Failed to draw an image");

            return Result<Bitmap>.Success(Draw(wordsWithCount, visualizingSettings, rectangles));
        }

        private List<string> ProcessText(ITextProcessor[] textProcessors, List<string> words)
        {
            foreach (var processor in textProcessors)
            {
                var processedText = processor.ProcessText(words);
                if (processedText.IsSuccess)
                    words = processedText.Value.ToList();
            }

            return words;
        }

        private Result<Bitmap> GetScaleImage(RectangleF[] rectangles, ImageScaler imageScaler, VisualizingSettings settings)
        {
            var smallestSizeOfRectangles = imageScaler.GetMinPoints(rectangles);
            var unscaledImageSize = imageScaler.GetImageSizeWithRealSizeRectangles(rectangles, smallestSizeOfRectangles);

            if (!imageScaler.NeedScale(settings, unscaledImageSize))
                return Result<Bitmap>.Failure("Scaling is not required");

            var bitmap = imageScaler.DrawScaleCloud(settings, rectangles, unscaledImageSize, smallestSizeOfRectangles);

            if (bitmap == null)
                return Result<Bitmap>.Failure("Failed to draw an image");

            Console.WriteLine("The tag cloud is drawn");

            return Result<Bitmap>.Success(bitmap);
        }

        private Result<List<string>> ReadText(string filePath)
        {
            var words = new List<string>();
            var extension = Path.GetExtension(filePath);
            var uncorrectedExtension = true;

            foreach (var textReader in textReaders)
            {
                if (extension == textReader.GetFileExtension())
                {
                    uncorrectedExtension = false;
                    var text = textReader.ReadTextFromFile(filePath);
                    if (text.IsSuccess)
                    {
                        words = textReader.ReadTextFromFile(filePath).Value.ToList();
                        break;
                    }
                    else
                        return Result<List<string>>.Failure(text.Error);
                }
            }

            if (uncorrectedExtension)
                return Result<List<string>>.Failure(string.Format("File contains an unsuitable format for reading - {0}", extension));

            return Result<List<string>>.Success(words);
        }

        public Result<bool> SaveImage(Bitmap bitmap, VisualizingSettings visualizingSettings)
        {
            if (bitmap == null)
                return Result<bool>.Failure("Bitmap is null");

            var extension = Path.GetExtension(visualizingSettings.ImageName);
            var format = GetImageFormat(extension);

            if (!format.IsSuccess)
            {
                Console.WriteLine(format.Error);
                return Result<bool>.Failure("Uncorrected Image Format");
            }

            bitmap.Save(visualizingSettings.ImageName, format.Value);
            Console.WriteLine($"The image is saved, the path to the image: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase)}");
            return Result<bool>.Success(true);
        }

        private Result<ImageFormat> GetImageFormat(string fileName)
        {
            var extension = Path.GetExtension(fileName);

            if (string.IsNullOrEmpty(extension))
                return Result<ImageFormat>.Failure(string.Format("Unable to determine file extension for fileName: {0}", fileName));

            switch (extension.ToLower())
            {
                case @".bmp":
                    return Result<ImageFormat>.Success(ImageFormat.Bmp);

                case @".gif":
                    return Result<ImageFormat>.Success(ImageFormat.Gif);

                case @".ico":
                    return Result<ImageFormat>.Success(ImageFormat.Icon);

                case @".jpg":
                case @".jpeg":
                    return Result<ImageFormat>.Success(ImageFormat.Jpeg);

                case @".png":
                    return Result<ImageFormat>.Success(ImageFormat.Png);

                case @".tif":
                case @".tiff":
                    return Result<ImageFormat>.Success(ImageFormat.Tiff);

                case @".wmf":
                    return Result<ImageFormat>.Success(ImageFormat.Wmf);

                default:
                    return Result<ImageFormat>.Failure(string.Format("Unable to determine file extension for fileName: {0}", fileName));
            }
        }

        private Bitmap Draw(Dictionary<string, int> tags, VisualizingSettings settings, RectangleF[] rectangles)
        {
            var bitmap = new Bitmap(settings.ImageSize.Width, settings.ImageSize.Height);
            using var brush = new SolidBrush(settings.PenColor);
            using var graphics = Graphics.FromImage(bitmap);
            graphics.Clear(settings.BackgroundColor);

            for (var i = 0; i < rectangles.Length; i++)
                foreach (var tag in tags)
                {
                    var rectangle = rectangles[i];
                    var font = new Font(settings.Font, 24 + (tag.Value * 6));
                    graphics.DrawString(tag.Key, font, brush, rectangle.X, rectangle.Y);
                    i++;
                }

            Console.WriteLine("The tag cloud is drawn");

            return bitmap;
        }

        private RectangleF[] GetRectanglesToDraw(Dictionary<string, int> text, VisualizingSettings settings)
        {
            using var bitmap = new Bitmap(settings.ImageSize.Width, settings.ImageSize.Height);
            using var graphics = Graphics.FromImage(bitmap);
            var layouter = new CircularCloudLayouter(settings.PointDistributor);
            var rectangles = new List<RectangleF>();
            foreach (var line in text)
            {
                using var font = new Font(settings.Font, 24 + (line.Value * 6));
                SizeF size = graphics.MeasureString(line.Key, font);
                var rectangle = layouter.PutNextRectangle(size.ToSize());

                rectangles.Add(rectangle.Value);
            }

            return rectangles.ToArray();
        }
    }
}