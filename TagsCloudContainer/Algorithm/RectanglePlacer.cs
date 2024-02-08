using System.Diagnostics;
using TagsCloudContainer.Infrastucture;
using TagsCloudContainer.Infrastucture.Settings;

namespace TagsCloudContainer.Algorithm
{
    public sealed class RectanglePlacer : IRectanglePlacer
    {
        private readonly AlgorithmSettings algorithmSettings;
        private readonly ImageSettings imageSettings;

        public RectanglePlacer(AlgorithmSettings algorithmSettings, ImageSettings imageSettings)
        {
            this.algorithmSettings = algorithmSettings;
            this.imageSettings = imageSettings;
        }

        public Result<RectangleF> GetPossibleNextRectangle(List<TextRectangle> cloudRectangles, SizeF rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                return Result.Fail<RectangleF>("The width and height of the rectangle must be positive numbers");

            if (algorithmSettings.DeltaRadius == 0 || algorithmSettings.DeltaAngle == 0)
                return Result.Fail<RectangleF>("DeltaRadius and DeltaAngle cannot be equal to zero");

            var rect = FindPossibleNextRectangle(cloudRectangles, rectangleSize);
            return Result.Ok(rect);
        }

        private RectangleF FindPossibleNextRectangle(List<TextRectangle> cloudRectangles, SizeF rectangleSize)
        {
            var radius = algorithmSettings.Radius;
            var angle = algorithmSettings.Angle;
            var center = new Point(imageSettings.Width / 2, imageSettings.Height / 2);

            while (true)
            {
                var point = new Point(
                    (int)(center.X + radius * Math.Cos(angle)),
                    (int)(center.Y + radius * Math.Sin(angle))
                    );
                var possibleRectangle = new RectangleF(point, rectangleSize);

                if (!cloudRectangles.Any(textRectangle => textRectangle.Rectangle.IntersectsWith(possibleRectangle)))
                {
                    return possibleRectangle;
                }

                angle += algorithmSettings.DeltaAngle;
                radius += algorithmSettings.DeltaRadius;
            }
        }

    }
}
