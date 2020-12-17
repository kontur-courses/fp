using System.Collections.Generic;
using TagsCloudVisualization.AppSettings;
using TagsCloudVisualization.FontHandlers;
using TagsCloudVisualization.StringExtensions;
using TagsCloudVisualization.TagCloudLayouter;
using TagsCloudVisualization.Tags;
using TagsCloudVisualization.Words;

namespace TagsCloudVisualization.TagCloudBuilders
{
    public class TagCloudBuilder : ITagCloudBuilder
    {
        private readonly ICloudLayouter cloudLayouter;
        private readonly FontSettings fontSettings;

        public TagCloudBuilder(ICloudLayouter cloudLayouter, FontSettings fontSettings)
        {
            this.cloudLayouter = cloudLayouter;
            this.fontSettings = fontSettings;
        }

        public Result<IReadOnlyList<Tag>> Build(IEnumerable<Word> wordsFrequency)
        {
            cloudLayouter.ClearLayout();

            var tags = new List<Tag>();
            
            foreach (var word in wordsFrequency)
            {
                var fontCalculateResult = Result.Of(() => FontHandler.CalculateFont(word.Weight, fontSettings));
                var tagResult = fontCalculateResult
                    .Then(fnt => word.Value.MeasureString(fnt))
                    .Then(tagSize => cloudLayouter.PutNextRectangle(tagSize))
                    .Then(rect => new Tag(word.Value, rect, fontCalculateResult.Value));

                if (tagResult.IsSuccess)
                    tags.Add(tagResult.Value);
                else return Result.Fail<IReadOnlyList<Tag>>(tagResult.Error);
            }

            return tags;
        }
    }
}