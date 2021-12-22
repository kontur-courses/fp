using System;
using System.Collections.Generic;
using System.Drawing;
using TagCloud.layouter;
using TagCloud.repositories;
using TagCloud.settings;

namespace TagCloud.visual
{
    public class TagVisualizer : IVisualizer
    {
        private readonly List<Tag> tags = new();
        private readonly ITagSettings tagSettings;
        private readonly ICloudLayouter layouter;
        private readonly IWordHelper helper;

        public TagVisualizer(ITagSettings tagSettings, ICloudLayouter layouter, IWordHelper helper)
        {
            this.tagSettings = tagSettings;
            this.layouter = layouter;
            this.helper = helper;
        }

        public Result<IVisualizer> InitializeCloud(List<string> words)
        {
            var processWordsResult = ProcessWords(words, helper);
            if (!processWordsResult.IsSuccess)
                return Result.Fail<IVisualizer>(processWordsResult.Error);
            var statistics = helper.GetWordStatistics(processWordsResult.Value);
            var fillResult = Result.OfAction(() => FillTags(statistics));
            return fillResult.IsSuccess 
                ? Result.Ok<IVisualizer>(this) 
                : Result.Fail<IVisualizer>(ResultErrorType.InitializeCloudError);
        }
            
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

        private static Result<List<string>> ProcessWords(List<string> words, IWordHelper helper)
            => Result.Ok(words).Then(helper.ConvertWords).Then(helper.FilterWords);

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
                );
                tags.Add(new Tag(wordStatistic.Word, layoutRectangle, font));
            }
        }
    }
}