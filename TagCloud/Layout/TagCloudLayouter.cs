using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloud.Layout
{
    public class TagCloudLayouter : ITagCloudLayouter
    {
        private readonly ICloudLayouter layouter;

        public TagCloudLayouter(ICloudLayouter layouter)
        {
            this.layouter = layouter;
        }

        public Result<List<Tag>> GetLayout(ICollection<KeyValuePair<string, double>> words)
        {
            var result = new List<Tag>();
            foreach (var keyValuePair in words)
            {
                if (keyValuePair.Key == "")
                    continue;
                var width = (int) Math.Round(keyValuePair.Key.Length * keyValuePair.Value);
                var height = (int) Math.Round(keyValuePair.Value);
                var size = new Size(width, height);
                var nextRectangle = layouter.PutNextRectangle(size);
                if (nextRectangle.IsSuccess)
                {
                    var tag = new Tag(keyValuePair.Key, nextRectangle.Value);
                    result.Add(tag);
                }
            }

            return result;
        }
    }
}