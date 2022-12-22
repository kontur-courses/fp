using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Microsoft.VisualBasic;
using ResultOf;
using TagsCloudContainer.App.Layouter;
using TagsCloudContainer.Infrastructure;

namespace TagsCloudContainer.App.Layouter
{
    public class TagsLayouter
    {
        private ICircularCloudLayouter cloudLayouter;
        private readonly ITagsExtractor tagsExtractor;
        private readonly IImageHolder imageHolder;
        private readonly FontText fontText;

        public TagsLayouter(ICircularCloudLayouter cloudLayouter, ITagsExtractor tagsExtractor,
            FontText fontText, IImageHolder imageHolder)
        {
            this.cloudLayouter = cloudLayouter;
            this.tagsExtractor = tagsExtractor;
            this.imageHolder = imageHolder;
            this.fontText = fontText;
        }

        public Result<IEnumerable<TagInfo>> PutAllTags(string text)
        {
            cloudLayouter.Clear();

            var wordsWithCountRepeat = tagsExtractor.FindAllTagsInText(text).GetValueOrThrow();
            var minT = wordsWithCountRepeat.Values.Min();
            var maxT = wordsWithCountRepeat.Values.Max();
            return Result.Of(() => wordsWithCountRepeat.Select(p =>
                PutNextTag(p.Key, p.Value, minT, maxT).GetValueOrThrow()));
        }

        private Result<TagInfo> PutNextTag(string word, int countRepeat, int minT, int maxT)
        {
            var font = new Font(fontText.Font.FontFamily, CalculateSizeFont(countRepeat, minT, maxT, fontText.Font.Size), fontText.Font.Style);
            return Result.Of(() => cloudLayouter.PutNextRectangle(CalculateSizeWord(word, font)))
                .Then(CheckIsRectangleInsideArea)
                .Then(rect => new TagInfo(word, font, rect));
        }

        private Result<Rectangle> CheckIsRectangleInsideArea(Rectangle rect)
        {
            var size = imageHolder.GetImageSize();
            return rect.Left >=0 && rect.Right <= size.GetValueOrThrow().Width && rect.Top >= 0 && rect.Bottom <= size.GetValueOrThrow().Height
                ? Result.Ok(rect)
                : Result.Fail<Rectangle>("Облако тегов не влезло на изображение заданного размера");
        }

        private float CalculateSizeFont(int T, int minT, int maxT, float f)
        {
            if (maxT == minT) return f;
            return Math.Max(f * (T - minT) / (maxT - minT), 1);
        }

        private Size CalculateSizeWord(string word, Font font)
        {
            var graphics = imageHolder.StartDrawing();
            var sizeF = graphics.MeasureString(word, font);
            return new Size((int)Math.Ceiling(sizeF.Width), (int)Math.Ceiling(sizeF.Height));
        }
    }
}
