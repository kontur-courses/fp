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

        public void Draw()
        {
            var imageSize = config.ImageSize;
            using (var image = new Bitmap(imageSize.Width, imageSize.Height))
            using (var graphics = Graphics.FromImage(image))
            {
                graphics.Clear(Color.DarkKhaki);
                foreach (var wordCount in parser.GetWordsCounts())
                    DrawWord(wordCount, graphics);
                var savingName = savePath + "." + config.ImageFormat;
                image.Save(savingName, config.ImageFormat);
            }
        }

        private void DrawWord(KeyValuePair<string, int> wordCount, Graphics graphics)
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
            }
        }

        private int ScaleFontSize(int fontSize, int wordQuantity)
        {
            const double magicLogarithmBase = 1.02;
            return (int)Math.Ceiling(fontSize + Math.Log(wordQuantity, magicLogarithmBase));
        }
    }
}
