using System;
using System.Drawing;
using TagCloudContainer.Result;

namespace TagCloudContainer
{
    /// <summary>
    /// Вспомогательный класс вычисляющий координаты прямоугольников.
    /// </summary>
    public static class RectangleCoordinatesCalculator
    {
        public static Result<Point> CalculateRectangleCoordinates(Result<Point> rectangleCenter, Size rectangleSize)
        {
            if (!rectangleCenter.IsSuccess)
                return new Result<Point>(rectangleCenter.Error);
            if (rectangleSize.Height < 0 || rectangleSize.Width < 0)
                return new Result<Point>("Incorrect size of rectangle");
            return new Result<Point>(null,
                new Point(rectangleCenter.Value.X - rectangleSize.Width / 2,
                    rectangleCenter.Value.Y - rectangleSize.Height / 2));
        }
    }
}