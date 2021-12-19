using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudContainerTests.CloudLayouterTests
{
    public class RectanglesDrawer
    {
        public Bitmap DrawRectangles(IEnumerable<Rectangle> rectangles, Size imageSize)
        {
            var rectanglesList = rectangles.ToList();
            if (rectanglesList.Count == 0)
                throw new ArgumentException("Sequence does not contain any elements");
            
            var center = rectanglesList.First().Location;

            var image = new Bitmap(imageSize.Width, imageSize.Height);
            using var graph = Graphics.FromImage(image);

            var random = new Random();
            using var pen = new Pen(GetRandomColor(random), 1);

            graph.TranslateTransform(image.Width / 2 - center.X, image.Height / 2 - center.Y);

            foreach (var rectangle in rectanglesList)
            {
                pen.Color = GetRandomColor(random);
                graph.DrawRectangle(pen, rectangle);
            }

            return image;
        }

        private static Color GetRandomColor(Random random) =>
            Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
    }
}