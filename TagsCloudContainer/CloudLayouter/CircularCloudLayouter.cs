using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudContainer.CloudItem;
using TagsCloudContainer.Distribution;

namespace TagsCloudContainer.CloudLayouter
{
    public class CircularCloudLayouter : ICloudLayouter
    {
        public CircularCloudLayouter(IDistribution distribution)
        {
            Items = new List<ICloudItem>();
            Distribution = distribution;
        }

        private IDistribution Distribution { get; }
        public IList<ICloudItem> Items { get; }

        public Result<ICloudItem> PutNextCloudItem(string word, Size size, Font font)
        {
            if (size.Width < 0 || size.Height < 0)
                return Result.Fail<ICloudItem>("Height and width must be positive");

            foreach (var point in Distribution.GetPoints())
            {
                var location = new Point(new Size(point) - size / 2);
                var rectangle = new Rectangle(location, size);

                if (Items.All(item => !item.Rectangle.IntersectsWith(rectangle)))
                {
                    var item = new TagCloudItem(word, rectangle, font);
                    Items.Add(item);
                    return Result.Ok<ICloudItem>(item);
                }
            }

            return Result.Fail<ICloudItem>("The end of the distribution has been reached");
        }
    }
}