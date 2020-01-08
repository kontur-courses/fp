using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ResultOf;
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

        public Result<List<TagRectangle>> GetRectangles(Graphics graphics, ImageSettings imageSettings,
            string path = null)
        {
            layouter.Clear();
            var tagCollection = tagCollectionFactory.Create(imageSettings, path);
            if (!tagCollection.IsSuccess)
                return Result.Fail<List<TagRectangle>>(tagCollection.Error);
            var center = new Point(imageSettings.Width / 2, imageSettings.Height / 2);
            var rectangles = Result.Of(() => tagCollection.Value
                    .Select(t => new TagRectangle(
                        t,
                        layouter.PutNextRectangle(GetWordSize(t, graphics), center)))
                    .ToList())
                .Then(list =>
                {
                    if (!list.Any(r => IsRectangleInPolygon(r.Area, imageSettings.Width, imageSettings.Height)))
                        throw new ArgumentException("Облако не помещается в изображение");
                    return list;
                });
            return rectangles;
        }

        private static bool IsRectangleInPolygon(RectangleF rectangle, int width, int height)
        {
            return rectangle.Top > 0 && rectangle.Left > 0 &&
                   rectangle.Bottom < height && rectangle.Top > 0 &&
                   rectangle.Right < width && rectangle.Left > 0;
        }

        private static SizeF GetWordSize(Tag tag, Graphics graphics)
        {
            return graphics.MeasureString(tag.Text, tag.Font);
        }
    }
}