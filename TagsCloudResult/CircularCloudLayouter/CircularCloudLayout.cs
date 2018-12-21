using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudResult.CircularCloudLayouter
{
    public class CircularCloudLayout: IRectangleLayout
    {
        private readonly RectangleStorage rectangleStorage;
        private readonly List<Rectangle> rectangles;

        public CircularCloudLayout(RectangleStorage rectangleStorage)
        {
            this.rectangleStorage = rectangleStorage;
            rectangles = new List<Rectangle>();
        }

        public Result<Rectangle> PutNextRectangle(Size rectangleSize)
        {
            var result = rectangleStorage.PlaceNewRectangle(rectangleSize);
            rectangles.Add(result.Value);

            return result;
        }

        public List<Rectangle> GetRectangles()
        {
            return rectangles;
        }

        public List<Rectangle> GetCoordinatesToDraw()
        {
            var result = new List<Rectangle>();
            var minX = rectangles.GetMinX();
            var yHeight = rectangles.GetYHeight();

            foreach (var rectangle in rectangles)
            {
                var xShift = -minX;
                var yShift = -yHeight;
                var x = rectangle.X + xShift;
                var y = rectangle.Y + yShift - rectangle.Height;

                result.Add(new Rectangle(x, y, rectangle.Width, rectangle.Height));
            }

            return result;
        }

        public Size ImageSize()
        {
            return rectangles.GetImageSize();
        }
    }
}