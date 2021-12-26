using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization
{
    public class TagParser : ITagParser
    {
        private readonly ICloudLayouter cloudLayouter;
        private readonly Font font;
        private readonly Graphics graphics;

        public TagParser(ICloudLayouter cloudLayouter, Graphics graphics, Font font)
        {
            this.cloudLayouter = cloudLayouter;
            this.graphics = graphics;
            this.font = font;
        }

        public Result<IEnumerable<Tag>> ParseTags(IDictionary<string, int> freqDictionary)
        {
            var tagsList = new List<Tag>();
            foreach (var word in freqDictionary.Keys.OrderByDescending(x => freqDictionary[x]))
            {
                var size = graphics.MeasureString(word, font).ToSize();
                var rectangle = cloudLayouter.PutNextRectangle(size);
                if (rectangle.IsSuccess)
                    tagsList.Add(new Tag(rectangle.Value, word, freqDictionary[word]));
                else
                    return Result.Fail<IEnumerable<Tag>>(rectangle.Error);
            }

            return tagsList;
        }
    }
}