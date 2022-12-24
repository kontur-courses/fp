using System;
using System.Drawing;
using TagCloudContainer.TaskResult;

namespace TagCloudContainer
{
    /// <summary>
    /// Вспомогательный класс вычисляющий координаты прямоугольников.
    /// </summary>
    public static class RectangleCoordinatesCalculator
    {
        public static Result<Point> CalculateRectangleCoordinates(Result<Point> rectangleCenter, Size rectangleSize)
        {
            return rectangleCenter.Then(center =>
                rectangleSize.Height < 0 || rectangleSize.Width < 0
                    ? Result.OnFail<Point>("Incorrect size of rectangle")
                    : Result.OnSuccess(new Point(center.X - rectangleSize.Width / 2,
                        center.Y - rectangleSize.Height / 2)));
        }
    }
}