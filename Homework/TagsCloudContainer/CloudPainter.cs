using System;
using System.Collections.Generic;
using System.Drawing;
using TagsCloudContainer.CloudLayouters;
using TagsCloudContainer.PaintConfigs;
using TagsCloudContainer.TextParsers;


namespace TagsCloudContainer
{
    public class CloudPainter
    {
        private readonly ICloudLayouter cloudLayouter;
        private readonly IPaintConfig config;
        private readonly ITextParser parser;
        private readonly string savePath;
        private const int WordsBorder = 2;

        public CloudPainter(ICloudLayouter cloudLayouter, IPaintConfig config,
            ITextParser parser, string savePath)
        {
            this.cloudLayouter = cloudLayouter;
            this.config = config;
            this.parser = parser;
            this.savePath = savePath;
        }

        public Result<string> Draw()
        {
            var imageSize = config.ImageSize;
            var savingName = savePath + "." + config.ImageFormat;
            var imageRect = new Rectangle(new Point(0, 0), config.ImageSize);
            var drawingResult = $"Image successfully saved in {savingName}".AsResult();

            using (var image = new Bitmap(imageSize.Width, imageSize.Height))
            using (var graphics = Graphics.FromImage(image))
            {
                graphics.Clear(Color.DarkKhaki);
                foreach (var wordCount in parser.GetWordsCounts())
                    drawingResult = DrawWord(wordCount, graphics, drawingResult, imageRect);
                image.Save(savingName, config.ImageFormat);
            }

            return drawingResult;
        }

        private Result<string> DrawWord(KeyValuePair<string, int> wordCount, Graphics graphics,
            Result<string> drawingResult, Rectangle imageRect)
        {
            var word = wordCount.Key;
            var scaledFontSize = ScaleFontSize(config.FontSize, wordCount.Value);
            using (var drawFont = new Font(config.FontName, scaledFontSize))
            {
                var rectSize = graphics.MeasureString(word, drawFont);
                var drawBrush = config.Color.GetNextColor();
                var enclosingRectangle = cloudLayouter.PutNextRectangle(
                    new Size((int)rectSize.Width + WordsBorder, (int)rectSize.Height + WordsBorder));
                graphics.DrawString(word, drawFont, drawBrush, enclosingRectangle);

                return GetDrawingResult(drawingResult, imageRect, enclosingRectangle);
            }
        }

        private Result<string> GetDrawingResult(Result<string> drawingResult, 
            Rectangle imageRect, Rectangle wordRect)
        {
            return imageRect.Contains(wordRect)
                ? drawingResult
                : Result.Fail<string>("One or more words are not inside the image!");
        }

        private int ScaleFontSize(int fontSize, int wordQuantity)
        {
            const double magicLogarithmBase = 1.02;
            return (int)Math.Ceiling(fontSize + Math.Log(wordQuantity, magicLogarithmBase));
        }
    }
}
