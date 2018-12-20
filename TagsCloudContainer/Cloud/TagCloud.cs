using System.Collections.Generic;
using System.Drawing;
using ResultOf;
using TagsCloudContainer.Words;
using TagsCloudVisualization;

namespace TagsCloudContainer.Cloud
{
    public class TagCloud
    {
        public Result<WordTag[]> Tags { get; }
        private const int letterWidthInPixels = 20;
        public TagCloud(ICloudLayouter cloudLayouter, WordAnalizer wordAnalizer)
        {
            Tags = wordAnalizer.WordPacks.Then(wordPacks =>
            {
                var tags = new List<WordTag>();
                foreach (var pack in wordPacks)
                {
                    var word = pack.Key;
                    var size = new Size(word.Length * letterWidthInPixels * pack.Count,
                        letterWidthInPixels * 2 * pack.Count);
                    var rect = cloudLayouter.PutNextRectangle(size);
                    tags.Add(new WordTag(rect, word));
                }

                return tags.ToArray();
            }).RefineError("Tag cloud init error");
        }
    }
}
