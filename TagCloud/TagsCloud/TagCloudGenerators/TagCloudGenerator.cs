using System.Collections.Generic;
using System.Drawing;
using TagsCloud.Interfaces;
using TagsCloud.TagGenerators;
using TagsCloud.ErrorHandling;

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
            using (var image = new Bitmap(1, 1))
            using (var graph = Graphics.FromImage(image))
            {
                foreach (var tag in allTags)
                {
                    using (var font = new Font(tag.font.fontName, tag.font.fontSize))
                    {
                        var size = graph.MeasureString(tag.word, font).ToSize();
                        result.Add((tag, tagCloud.PutNextRectangle(size)));
                    }
                }
            }
            return ((IEnumerable<(Tag tag, Rectangle position)>)result).AsResult();
        }
    }
}
