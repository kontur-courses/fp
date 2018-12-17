using System;
using System.Drawing;
using TagsCloudResult.Settings;

namespace TagsCloudResult.Algorithms
{
    public class ArchimedeanSpiral : ISpiral
    {
        private readonly ICloudSettings cloudSettings;
        private double spiralAngle;

        public ArchimedeanSpiral(ICloudSettings cloudSettings)
        {
            this.cloudSettings = cloudSettings;
        }

        public Result<Point> GetNextPoint()
        {
            return Result.Of(GenerateCoordinates)
                .Then(coords => new Point(coords.Value.x, coords.Value.y))
                .RefineError("Не удалось получить следующую точку для размещения прямоугольника");
        }

        public double GetCurrentSpiralAngle() => spiralAngle;

        private Result<(int x, int y)> GenerateCoordinates()
        {
            spiralAngle++;
            return Result.Of(() => (x: cloudSettings.CenterPoint.X + (int) (spiralAngle * Math.Cos(spiralAngle)),
                y: cloudSettings.CenterPoint.Y + (int) (spiralAngle * Math.Sin(spiralAngle))));
        }
    }
}
