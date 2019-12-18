using System;
using System.Collections.Generic;
using System.Drawing;
using TagsCloud.ErrorHandling;
using TagsCloud.Interfaces;
using TagsCloud.TagGenerators;

namespace TagsCloud.TagCloudGenerators
{
    public class TagCloudGenerator : ITagCloudGenerator
    {
        private readonly ITagCloudLayouter tagCloud;

        public TagCloudGenerator(ITagCloudLayouter tagCloud)
        {
            this.tagCloud = tagCloud;
        }

        public Result<IEnumerable<(Tag tag, Rectangle position)>> GenerateTagCloud(IEnumerable<Tag> allTags)
        {
            var result = new List<(Tag tag, Rectangle position)>();
            var errors = new List<string>();
            using (var image = new Bitmap(1, 1))
            {
                using var graph = Graphics.FromImage(image);
                foreach (var tag in allTags)
                {
                    using var font = new Font(tag.fontSettings.fontFamily, tag.fontSettings.fontSize);
                    var size = graph.MeasureString(tag.word, font).ToSize();
                    var positionForNextRectangle = tagCloud.PutNextRectangle(size)
                        .OnFail(errors.Add);
                    if (positionForNextRectangle.IsSuccess)
                        positionForNextRectangle.Then(position => result.Add((tag, position)));
                }
            }

            return errors.Count != 0
                ? Result.Fail<IEnumerable<(Tag tag, Rectangle position)>>(string.Join(Environment.NewLine, errors))
                : ((IEnumerable<(Tag tag, Rectangle position)>) result).AsResult();
        }
    }
}