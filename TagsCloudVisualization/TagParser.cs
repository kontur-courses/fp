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
            return freqDictionary == null
                ? Result.Fail<IEnumerable<Tag>>("freqDictionary was null")
                : Result.Ok(CreateTags(freqDictionary));
        }

        private IEnumerable<Tag> CreateTags(IDictionary<string, int> freqDictionary)
        {
            foreach (var word in freqDictionary.Keys.OrderByDescending(x => freqDictionary[x]))
            {
                var rectangle = cloudLayouter.PutNextWord(word, font, graphics);
                yield return new Tag(rectangle.Value, word, freqDictionary[word]);
            }
        }
    }
}