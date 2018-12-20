using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TagsCloudContainer.Drawing;
using TagsCloudContainer.Extensions;

namespace TagsCloudContainer.Layout
{
    public class WordLayout : IWordLayout
    {
        private readonly IRectangleLayout layout;
        private readonly ImageSettings settings;

        private readonly HashSet<Tag> tags;
        public IReadOnlyCollection<Tag> Tags => tags;

        public WordLayout(IRectangleLayout layout, ImageSettings settings)
        {
            this.layout = layout;
            this.settings = settings;
            tags = new HashSet<Tag>();
        }

        public void PlaceWords(Dictionary<string, int> wordWeights)
        {
            var weightSum = wordWeights.Sum(p => p.Value);
            wordWeights.ToList().Sort((p1, p2) => p1.Value.CompareTo(p2.Value));

            foreach (var pair in wordWeights)
            {
                var fontSize = Math.Max((float) pair.Value / weightSum * settings.MaxFontSize, settings.MinFontSize);
                var font = settings.TextFont.SetSize(fontSize);

                var rectangle = layout.PutNextRectangle(TextRenderer.MeasureText(pair.Key, font));
                tags.Add(new Tag(pair.Key, font, rectangle));
            }
        }
    }
}