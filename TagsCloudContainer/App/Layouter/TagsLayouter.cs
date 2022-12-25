using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ResultOf;
using TagsCloudContainer.Infrastructure;

namespace TagsCloudContainer.App.Layouter
{
    public class TagsLayouter
    {
        private ICircularCloudLayouter cloudLayouter;
        private readonly ITagsExtractor tagsExtractor;
        private readonly FontText fontText;

        public TagsLayouter(ICircularCloudLayouter cloudLayouter, ITagsExtractor tagsExtractor,
            FontText fontText)
        {
            this.cloudLayouter = cloudLayouter;
            this.tagsExtractor = tagsExtractor;
            this.fontText = fontText;
        }

        public Result<IEnumerable<TagInfo>> PutAllTags(string text, Size imageSize)
        {
            cloudLayouter.Clear();
            return tagsExtractor.FindAllTagsInText(text)
                .Then(t => t.Select(p => PutNextTag(p.Key, p.Value, t.Values.Min(), t.Values.Max(), imageSize)));
        }

        private TagInfo PutNextTag(string word, int countRepeat, int minT, int maxT, Size imageSize)
        {
            var font = new Font(fontText.Font.FontFamily, CalculateSizeFont(countRepeat, minT, maxT, fontText.Font.Size), fontText.Font.Style);
            var rectangle = cloudLayouter.PutNextRectangle(CalculateSizeWord(word, font));
            return new TagInfo(word, font, rectangle);
        }

        private float CalculateSizeFont(int T, int minT, int maxT, float f)
        {
            if (maxT == minT) return f;
            return Math.Max(f * (T - minT) / (maxT - minT), 1);
        }

        private Size CalculateSizeWord(string word, Font font)
        {
            using Graphics graphics = Graphics.FromImage(new Bitmap(100, 100));
            var sizeF = graphics.MeasureString(word, font);
            return new Size((int)Math.Ceiling(sizeF.Width), (int)Math.Ceiling(sizeF.Height));
        }
    }
}
