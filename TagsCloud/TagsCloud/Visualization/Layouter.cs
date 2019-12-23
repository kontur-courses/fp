using System.Collections.Generic;
using System.Linq;
using TagsCloud.CloudConstruction;
using TagsCloud.ErrorHandler;
using TagsCloud.Visualization.SizeDefiner;

namespace TagsCloud.Visualization
{
    public class Layouter : ILayouter
    {
        private readonly ICloudLayouter _cloud;
        private readonly ISizeDefiner _sizeDefiner;

        public Layouter(ICloudLayouter cloud, ISizeDefiner sizeDefiner)
        {
            _cloud = cloud;
            _sizeDefiner = sizeDefiner;
        }

        public Result<IEnumerable<Tag.Tag>> GetTags(Dictionary<string, int> wordFrequency)
        {
            var maxFrequency = wordFrequency.Values.Max();
            var minFrequency = wordFrequency.Values.Min();
            var result = new List<Tag.Tag>();
            foreach (var item in wordFrequency)
            {
                var tagSize = _sizeDefiner.GetTagSize(item.Key, item.Value, minFrequency, maxFrequency);
                var locationRectangle = _cloud.PutNextRectangle(tagSize.RectangleSize);
                if (!locationRectangle.IsSuccess) return Result.Fail<IEnumerable<Tag.Tag>>(locationRectangle.Error);

                result.Add(new Tag.Tag(locationRectangle.Value, item.Key, tagSize.FontSize, item.Value));
            }

            return result;
        }
    }
}