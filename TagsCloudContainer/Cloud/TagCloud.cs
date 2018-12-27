using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ResultOf;
using TagsCloudContainer.Words;
using TagsCloudVisualization;

namespace TagsCloudContainer.Cloud
{
    public class TagCloud : ITagCloud
    {
        public Result<WordTag[]> Tags { get; }
        private const int letterWidthInPixels = 20;
        public TagCloud(ICloudLayouter cloudLayouter, IWordAnalyzer wordAnalizer) =>
            Tags = wordAnalizer.WordPacks
                .Then(wordPacks => wordPacks
                    .Select(pack => new WordTag(cloudLayouter
                    .PutNextRectangle(new Size(pack.Key.Length * letterWidthInPixels * pack.Count,
                        letterWidthInPixels * 2 * pack.Count)), pack.Key)).ToArray())
                .RefineError("Tag cloud init error");
    }
}
