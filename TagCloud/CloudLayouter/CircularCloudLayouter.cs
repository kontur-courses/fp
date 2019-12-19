using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagCloud.CloudLayouter.FigurePaths;
using TagCloud.CloudVisualizerSpace.CloudViewConfigurationSpace;

namespace TagCloud.CloudLayouter
{
    public class CircularCloudLayouter : ICloudLayouter
    {
        public IEnumerable<Rectangle> Rectangles => rectangles;
        public Rectangle WrapperRectangle => wrapperRectangle;

        private readonly List<Rectangle> rectangles;

        private readonly CloudViewConfiguration configuration;
        private readonly IFigurePath figurePath;
        private Rectangle wrapperRectangle;


        public CircularCloudLayouter(CloudViewConfiguration configuration)
        {
            this.configuration = configuration;
            rectangles = new List<Rectangle>();
            figurePath = configuration.FigurePath.GetFigurePath();
        }

        public Result<Rectangle> PutNextRectangle(Size rectangleSize)
        {
            var center = new Size(configuration.CloudCenter); // Сделано чтобы проще прибавлять к точке
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                return Result.Fail<Rectangle>("Directions should be non-negative");

            var rectangle = new Rectangle(figurePath.GetNextPoint() + center, rectangleSize);

            while (rectangles.Any(rect => rect.IntersectsWith(rectangle)))
            {
                rectangle = new Rectangle(figurePath.GetNextPoint() + center, rectangleSize);
            }

            if (configuration.NeedSnuggle)
                rectangle = SnuggleRectangle(rectangle);

            rectangles.Add(rectangle);
            UpdateCloudRectangle(rectangle);

            return Result.Ok(rectangle);
        }

        private Rectangle SnuggleRectangle(Rectangle rectangle)
        {
            var deltaX = Math.Sign(configuration.CloudCenter.X - rectangle.X);
            var deltaY = Math.Sign(configuration.CloudCenter.Y - rectangle.Y);
            while (deltaX != 0 || deltaY != 0)
            {
                rectangle.X += deltaX;
                if (deltaX != 0 && !rectangles.Any(rect => rect.IntersectsWith(rectangle)))
                {
                    deltaX = Math.Sign(configuration.CloudCenter.X - rectangle.X);
                    continue;
                }

                rectangle.X -= deltaX;
                rectangle.Y += deltaY;
                if (deltaY != 0 && !rectangles.Any(rect => rect.IntersectsWith(rectangle)))
                {
                    deltaY = Math.Sign(configuration.CloudCenter.Y - rectangle.Y);
                    continue;
                }

                rectangle.Y -= deltaY;
                break;
            }

            return rectangle;
        }

        private void UpdateCloudRectangle(Rectangle rectangle)
        {
            if (rectangle.X < wrapperRectangle.X)
            {
                wrapperRectangle.Width += wrapperRectangle.X - rectangle.X;
                wrapperRectangle.X = rectangle.X;
            }

            if (rectangle.Y < wrapperRectangle.Y)
            {
                wrapperRectangle.Height += wrapperRectangle.Y - rectangle.Y;
                wrapperRectangle.Y = rectangle.Y;
            }

            if (rectangle.Right > wrapperRectangle.Right)
            {
                wrapperRectangle.Width += rectangle.Right - wrapperRectangle.Right;
            }

            if (rectangle.Bottom > wrapperRectangle.Bottom)
            {
                wrapperRectangle.Height += rectangle.Bottom - wrapperRectangle.Bottom;
            }
        }
    }
}
