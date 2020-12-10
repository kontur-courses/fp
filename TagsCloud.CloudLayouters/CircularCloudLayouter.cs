using System.Collections.Generic;
using System.Drawing;
using TagsCloud.Common;
using TagsCloud.ResultPattern;

namespace TagsCloud.CloudLayouters
{
    public class CircularCloudLayouter : ICircularCloudLayouter
    {
        private readonly ISpiral spiral;
        private readonly List<Rectangle> rectangles = new List<Rectangle>();

        public CircularCloudLayouter(ISpiral spiral)
        {
            this.spiral = spiral;
        }

        public Result<Rectangle> PutNextRectangle(Size rectSize)
        {
            if (rectSize.Width <= 0 || rectSize.Height <= 0)
                return Result.Fail<Rectangle>("rectangle's width and height must be positive numbers");

            var rect = GetNextRectangle(rectSize);
            rectangles.Add(rect);
            return rect.AsResult();
        }

        private Rectangle GetNextRectangle(Size rectSize)
        {
            while (true)
            {
                var rectLocation = spiral.GetNextPoint() - new Size(rectSize.Width / 2, rectSize.Height / 2);
                var rect = new Rectangle(rectLocation, rectSize);
                if (!rect.IntersectsWith(rectangles))
                    return rect;
            }
        }
    }
}
