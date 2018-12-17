using System;
using System.Collections.Generic;
using System.Drawing;
using TagsCloudVisualization.TagsCloud.CloudConstruction;

namespace TagsCloudVisualization.TagsCloud.CircularCloud
{
    public class CircularCloudLayouter
    {
        public Size WindowSize { get; set; }
        public Point Center { get; set; }
        public List<Rectangle> Rectangles { get; set; }
        public CloudCompactor CloudCompactor { get; set; }
        public RectangleGenerator RectangleGenerator { get; set; }
        public static bool IsCompressedCloud { get; set; }

        public CircularCloudLayouter(Point center, Size windowSize)
        {
            Center = center;
            WindowSize = windowSize;
            Rectangles = new List<Rectangle>();
            CloudCompactor = new CloudCompactor(this);
            RectangleGenerator = new RectangleGenerator(this);
        }

        public Rectangle PutNextRectangle(Size size)
        {
            var resultRect = RectangleGenerator.GetNextRectangle(size);
            if (IsCompressedCloud)
                resultRect = CloudCompactor.ShiftRectangleToTheNearest(resultRect);
            Rectangles.Add(resultRect);
            return resultRect;
        }

        public bool CheckPositionRectangle(Rectangle rectangle)
        {
            return rectangle.X < 0 || rectangle.Y < 0 ||
                   rectangle.X + rectangle.Width > WindowSize.Width ||
                   rectangle.Y + rectangle.Height > WindowSize.Height;
        }

        public void RefreshCircularCloudLayouter(Point center, Size windowSize)
        {
            Center = center;
            WindowSize = windowSize;
            Rectangles = new List<Rectangle>();
            RectangleGenerator = new RectangleGenerator(this);

        }
    }
}