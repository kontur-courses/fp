using System;
using System.Collections.Generic;
using System.Drawing;
using TagCloud.layouter;
using TagCloud.repositories;
using TagCloud.settings;

namespace TagCloud.visual
{
    public class TagCloudVisualizer : ICloudVisualizer
    {
        private readonly List<Tag> tags = new();
        private readonly ITagSettings tagSettings;
        private readonly ICloudLayouter layouter;
        private readonly IWordHelper helper;

        public TagCloudVisualizer(ITagSettings tagSettings, ICloudLayouter layouter, IWordHelper helper)
        {
            this.tagSettings = tagSettings;
            this.layouter = layouter;
            this.helper = helper;
        }

        public Result<ICloudVisualizer> InitializeCloud(IEnumerable<string> words)
            => ProcessWords(words, helper)
                .Then(w => helper.GetWordStatistics(w))
                .Then(w =>
                    Result.OfAction(() => FillTags(w), ResultErrorType.InitializeCloudError)
                ).Then(r => new Result<ICloudVisualizer>(r.Error, this));

        public Result<Image> GetImage(IDrawSettings drawSettings)
        {
            var (width, height) = (drawSettings.GetSize().Width, drawSettings.GetSize().Height);
            var bitmap = new Bitmap(width, height);
            var graphics = Graphics.FromImage(bitmap);

            var random = new Random();
            var offset = new Point(width / 2, height / 2);
            graphics.Clear(drawSettings.GetBackgroundColor());
            foreach (var tag in tags)
            {
                var brush = new SolidBrush(GetRandomColor(drawSettings.GetInnerColors(), random));
                var layoutRectangle = tag.LayoutRectangle;
                layoutRectangle.Offset(offset);
                graphics.DrawString(tag.Text, tag.Font, brush, layoutRectangle);
            }

            return bitmap;
        }

        private static Color GetRandomColor(IReadOnlyList<Color> colors, Random random)
            => colors[random.Next(colors.Count)];

        private static Result<IEnumerable<string>> ProcessWords(IEnumerable<string> words, IWordHelper helper)
            => helper.ConvertWords(words).Then(helper.FilterWords);

        private void FillTags(IEnumerable<WordStatistic> wordStatistics)
        {
            foreach (var wordStatistic in wordStatistics)
            {
                var font = new Font(
                    tagSettings.GetFontFamily(),
                    wordStatistic.Count * tagSettings.GetStartSize()
                );
                var layoutRectangle = layouter.PutNextRectangle(
                    new Size((int)font.Size * wordStatistic.Word.Length, font.Height)
                ).Value;
                tags.Add(new Tag(wordStatistic.Word, layoutRectangle, font));
            }
        }
    }
}