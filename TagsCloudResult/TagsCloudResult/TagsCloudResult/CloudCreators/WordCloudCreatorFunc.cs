using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudContainer
{
    public class WordCloudCreatorFunc<T>
    {
        private Func<Size, Rectangle> getNextRectangle;
        private Func<string, Font, SizeF> getSizeText;
        private Random rnd;

        public WordCloudCreatorFunc(Func<Size, Rectangle> getNextRectangle, Func<string, Font, SizeF> getSizeText)
        {
            this.getNextRectangle = getNextRectangle;
            this.getSizeText = getSizeText;
            rnd = new Random();
        }

        public IEnumerable<Word> CreateWordCloud<T>(Dictionary<T, int> wordsAndFrequency, ImageSettings imageSettings)
        {
            var words = wordsAndFrequency.OrderBy(word => rnd.Next());

            if (!words.Any())
                yield break;

            var imageCenterX = imageSettings.ImageSize.Width / 2;
            var imageCenterY = imageSettings.ImageSize.Height / 2;
            var sizeCoef = (int)(imageSettings.ImageSize.Width / words.Select(word => word.Key.ToString().Length).Sum() * Math.PI * 2);

            foreach (var word in words)
            {
                var fontSize = sizeCoef + word.Value;
                var font = new Font(imageSettings.FontFamily, fontSize, imageSettings.FontStyle, GraphicsUnit.Pixel);

                var wordSize = getSizeText(word.Key.ToString(), font);

                var rect = getNextRectangle(new Size((int)Math.Ceiling(wordSize.Width), (int)Math.Ceiling(wordSize.Height)));
                rect.X += imageCenterX;
                rect.Y += imageCenterY;

                yield return new Word(rect, word.Key.ToString(), font);
            }
        }
    }
}
