using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using TagCloud.IServices;
using TagCloud.Models;

namespace TagCloud
{
    public class Cloud : ICloud
    {
        private readonly ICircularCloudLayouter layouter;
        private readonly ITagCollectionFactory tagCollectionFactory;

        public Cloud(ICircularCloudLayouter layouter, ITagCollectionFactory tagCollectionFactory)
        {
            this.tagCollectionFactory = tagCollectionFactory;
            this.layouter = layouter;
        }

        public List<TagRectangle> GetRectangles(Graphics graphics, ImageSettings imageSettings, string path = null)
        {
            layouter.Clear();
            var tagCollection = tagCollectionFactory.Create(imageSettings, path);
            if(!tagCollection.IsSuccess)
                throw new FileNotFoundException(tagCollection.Error);
            var center = new Point(imageSettings.Width / 2, imageSettings.Height / 2);
            var rectangles = tagCollection.Value
                .Select(t => new TagRectangle(
                    t,
                    layouter.PutNextRectangle(GetWordSize(t, graphics), center)))
                .ToList();
            return rectangles;
        }

        private SizeF GetWordSize(Tag tag, Graphics graphics)
        {
            return graphics.MeasureString(tag.Text, tag.Font);
        }
    }
}