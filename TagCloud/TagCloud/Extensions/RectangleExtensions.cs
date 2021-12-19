using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ResultOf;

namespace TagCloud.Extensions
{
    internal static class RectangleExtensions
    {
        /// <summary>
        ///     Находит расстояние от точки внутри прямоугольника до каждой его стороны
        /// </summary>
        /// <returns>Возвращает расстояния в порядке left, top, right, bottom (все расстояния положительные)</returns>
        public static Result<List<int>> GetDistancesToInnerPoint(this Rectangle rect, Point point)
        {
            var left = point.X - rect.Left;
            var top = point.Y - rect.Top;
            var right = rect.Right - point.X;
            var bottom = rect.Bottom - point.Y;

            var distances = new List<int> {left, top, right, bottom};
            return distances.Any(d => d < 0)
                ? Result.Fail<List<int>>("Точка расположена вне прямоугольника")
                : distances.AsResult();
        }
    }
}