using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CSharpFunctionalExtensions;
using TagsCloudContainer.Settings;

namespace TagsCloudContainer.Layout
{
    public class WordLayout : IWordLayout
    {
        private const int WidthAddition = 3;

        private readonly IRectangleLayout layout;
        private readonly ImageSettings settings;


        public WordLayout(IRectangleLayout layout, ImageSettings settings)
        {
            this.layout = layout;
            this.settings = settings;
        }

        public Result<HashSet<Tag>> PlaceWords(Dictionary<string, int> wordWeights)
        {
            var weightSum = wordWeights.Sum(p => p.Value);
            var tags = new HashSet<Tag>();
            wordWeights.ToList().Sort((p1, p2) => p1.Value.CompareTo(p2.Value));

            foreach (var pair in wordWeights.Take(settings.WordCount))
            {
                var fontSize = Math.Max((float) pair.Value / weightSum * settings.MaxFontSize, settings.MinFontSize);
                var font = new Font(settings.FontFamily, fontSize);

                var rectangle = layout.PutNextRectangle(MeasureWord(pair.Key, font));

                if (rectangle.IsSuccess)
                    tags.Add(new Tag(pair.Key, font, rectangle.Value));
            }

            return Result.Ok(tags);
        }

        private static Size MeasureWord(string word, Font font)
        {
            var size = TextRenderer.MeasureText(word, font);
            return new Size(size.Width + WidthAddition, size.Height);
        }
    }
}