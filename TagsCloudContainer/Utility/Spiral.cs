using System.Drawing;
using TagsCloudContainer.Interfaces;

namespace TagsCloudContainer.Utility
{
    public class Spiral : INextPointProvider
    {
        private readonly Point center;
        private readonly double angleStep;
        private readonly double radiusStep;
        private double angle;
        private double radius;

        public Result<Point> GetNextPoint()
        {
            try
            {
                var pointResult = ConvertFromPolarToCartesian(angle, radius);

                if (pointResult.IsSuccess)
                {
                    var point = pointResult.Value;
                    point.Offset(center);
                    angle += angleStep;
                    radius += radiusStep;
                    return Result.Ok(point);
                }
                else
                {
                    return Result.Fail<Point>($"Error getting next point on spiral: {pointResult.Error}");
                }
            }
            catch (Exception ex)
            {
                return Result.Fail<Point>($"Error getting next point on spiral: {ex.Message}");
            }
        }

        public static Result<Spiral> Create(Point center, double angleStep, double radiusStep)
        {
            if (radiusStep <= 0 || angleStep <= 0)
            {
                return Result.Fail<Spiral>("Step values should be positive.");
            }

            try
            {
                var spiral = new Spiral(center, angleStep, radiusStep);
                return Result.Ok(spiral);
            }
            catch (Exception ex)
            {
                return Result.Fail<Spiral>($"Error creating Spiral: {ex.Message}");
            }
        }

        public Spiral(Point center, double angleStep, double radiusStep)
        {
            if (angleStep <= 0 || radiusStep <= 0)
            {
                throw new ArgumentException("Step values should be positive.");
            }

            this.center = center;
            this.angleStep = angleStep;
            this.radiusStep = radiusStep;
        }

        public Result<IEnumerable<Point>> GetPointsOnSpiral()
        {
            try
            {
                var points = GeneratePointsOnSpiral().ToList();
                return Result.Ok<IEnumerable<Point>>(points);
            }
            catch (Exception ex)
            {
                return Result.Fail<IEnumerable<Point>>($"Error generating points on spiral: {ex.Message}");
            }
        }


        private IEnumerable<Point> GeneratePointsOnSpiral()
        {
            double angle = 0;
            double radius = 0;

            while (true)
            {
                var pointResult = ConvertFromPolarToCartesian(angle, radius);

                if (pointResult.IsSuccess)
                {
                    var point = pointResult.Value;
                    point.Offset(center);
                    yield return point;
                }
                else
                {
                    Console.WriteLine($"Error generating point on spiral: {pointResult.Error}");
                    yield break;
                }

                angle += angleStep;
                radius += radiusStep;
            }
        }

        private static Result<Point> ConvertFromPolarToCartesian(double angle, double radius)
        {
            try
            {
                var x = (int)Math.Round(Math.Cos(angle) * radius);
                var y = (int)Math.Round(Math.Sin(angle) * radius);
                return Result.Ok(new Point(x, y));
            }
            catch (Exception ex)
            {
                return Result.Fail<Point>($"Error converting from polar to cartesian: {ex.Message}");
            }
        }
    }
}
