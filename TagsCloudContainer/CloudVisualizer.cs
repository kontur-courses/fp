using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using TagsCloudVisualization;

namespace TagsCloudContainer
{
    public class CloudVisualizer : ICloudVisualizer
    {
        private readonly Result<IEnumerable<KeyValuePair<string,int>>> wordsWithFontSize;
        private readonly CircularCloudLayouter layouter = new CircularCloudLayouter(new Point(1920 / 2, 1080 / 2));
        private readonly CloudOptions options;
        private readonly HashSet<Rectangle> rectangles = new HashSet<Rectangle>();

        public CloudVisualizer(ICloudConfigurator configurator, CloudOptions options)
        {
            this.options = options;
            wordsWithFontSize = configurator.ConfigureCloud();
        }

        public Result<None> VisualizeCloud()
        {
            var imageFormat = GetImageFormat(options.Format);
            var filename = $"wordsCloud.{imageFormat.ToString().ToLower()}";

            if (!wordsWithFontSize.IsSuccess)
                return Result.Fail<None>(wordsWithFontSize.Error);
            if (!Color.FromName(options.Color).IsKnownColor && !Color.FromName(options.Color).IsSystemColor)
                return Result.Fail<None>($"Color {options.Color} is unknown");

            return Result.Of(RenderToBitmap)
                .Then(bitmap => bitmap.Save(filename, imageFormat));
        }

        private Bitmap RenderToBitmap()
        {
            var size = new Size(int.Parse(options.Width), int.Parse(options.Height));
            var bitmap = new Bitmap(size.Width, size.Height);
            var graphics = Graphics.FromImage(bitmap);

            graphics.Clear(Color.White);
            foreach (var word in wordsWithFontSize.Value)
            {
                DrawWord(word.Key, word.Value, graphics);
            }

            CheckImageSize();
            return bitmap;
        }

        private void DrawWord(string word, int fontSize, Graphics graphics)
        {
            var font = new Font(new FontFamily(options.FontFamily), fontSize);
            var size = graphics.MeasureString(word, font);
            var intSize = new Size((int)size.Width + 1, (int)size.Height + 1);
            var rectangle = layouter.PutNextRectangle(intSize);
            graphics.DrawString(word, font, new SolidBrush(Color.FromName(options.Color)), rectangle);
            rectangles.Add(rectangle);
        }

        private ImageFormat GetImageFormat(string format)
        {
            PropertyInfo propertyInfo = typeof(ImageFormat)
                .GetProperties()
                .FirstOrDefault(p => p.Name.Equals(format, StringComparison.InvariantCultureIgnoreCase));
            if (propertyInfo != null)
                return propertyInfo.GetValue(propertyInfo) as ImageFormat;

            return ImageFormat.Png;
        }

        private void CheckImageSize()
        {
            var maxRight = rectangles.Max(r => r.Right);
            var maxBottom = rectangles.Max(r => r.Bottom);
            var errorMessage = string.Empty;
            if (int.Parse(options.Width) < maxRight)
                errorMessage += $"Width must be more than {maxRight}. ";
            if (int.Parse(options.Height) < maxBottom)
                errorMessage += $"Height must be more than {maxBottom}.";
            if (errorMessage != string.Empty)
                throw new Exception(errorMessage);
        }
    }
}
