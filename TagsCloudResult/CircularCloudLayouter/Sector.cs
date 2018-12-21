using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudResult.CircularCloudLayouter
{
    public class Sector
    {
        private readonly Point center;
        private readonly List<Point> points;
        private int xCoefficient;
        private int yCoefficient;

        public Sector(Quadrant quadrant, Point center)
        {
            points = new List<Point>();
            this.center = center;
            AddPoint(0, 0);

            InitCoefficients(quadrant);
        }

        private void InitCoefficients(Quadrant quadrant)
        {
            switch (quadrant)
            {
                case Quadrant.First:
                    xCoefficient = 1;
                    yCoefficient = 1;
                    break;
                case Quadrant.Second:
                    xCoefficient = -1;
                    yCoefficient = 1;
                    break;
                case Quadrant.Third:
                    xCoefficient = -1;
                    yCoefficient = -1;
                    break;
                case Quadrant.Fourth:
                    xCoefficient = 1;
                    yCoefficient = -1;
                    break;
            }
        }

        public Result<Rectangle> PlaceRectangle(double direction, Size rectangleSize)
        {
            var availablePoint = FindAvailablePoint(direction);
            RecalculatePointsInSector(availablePoint, rectangleSize);

            var rectangleLocation = FormatPoint(availablePoint, rectangleSize);

            return new Rectangle(rectangleLocation, rectangleSize).AsResult();
        }

        private double MakeDirectionRelative(double direction)
        {
            return direction % (Math.PI / 2);
        }

        private Point FindAvailablePoint(double direction)
        {
            direction = MakeDirectionRelative(direction);
            for (var index = 0; index < PointsNumber(); index++)
            {
                var point = points[index];
                if (IsPointOnDirectLine(point.X, point.Y, direction))
                    return point;

                if (IsPointUnderDirectLine(point.X, point.Y, direction))
                    return points[index - 1].X == point.X ? point : points[index - 1];
            }

            return new Point(0, 0);
        }

        public void RecalculatePointsInSector(Point downLeftPoint, Size rectangleSize)
        {
            var maxX = downLeftPoint.X + rectangleSize.Width;
            var maxY = downLeftPoint.Y + rectangleSize.Height;

            var rangeToRemove = FindPointsRangeToRemove(points, maxX, maxY);
            var pointsToInsert = FindPointsToInsert(rectangleSize, points, rangeToRemove, maxX, maxY);

            RemovePointsUnderNewRectangle(rangeToRemove);
            InsertNewPointsCreatedByRectangle(rangeToRemove.Item1, pointsToInsert);
        }

        public void InsertNewPointsCreatedByRectangle(int index, List<Point> pointsToInsert)
        {
            points.InsertRange(index, pointsToInsert);
        }

        public static List<Point> FindPointsToInsert(Size rectangleSize,
            List<Point> points, Tuple<int, int> rangeToRemove, int maxX, int maxY)
        {
            var pointsToInsert = new List<Point>();
            pointsToInsert.InsertRange(0, HandleLeftBorder(rectangleSize, points, rangeToRemove, maxX, maxY));
            pointsToInsert.InsertRange(pointsToInsert.Count,
                HandleRightBorder(rectangleSize, points, rangeToRemove, maxX, maxY));

            return pointsToInsert;
        }

        public static List<Point> HandleLeftBorder(Size rectangleSize, List<Point> points,
            Tuple<int, int> rangeToRemove, int maxX, int maxY)
        {
            var pointsToInsert = new List<Point>();
            var leftIndex = rangeToRemove.Item1;

            if (leftIndex == 0)
            {
                pointsToInsert.Add(new Point(0, maxY));
            }
            else
            {
                var firstPointToDelete = points[leftIndex];
                pointsToInsert.Add(
                    new Point(firstPointToDelete.X, firstPointToDelete.Y + rectangleSize.Height));
            }

            pointsToInsert.Add(new Point(maxX, maxY));

            return pointsToInsert;
        }

        public static List<Point> HandleRightBorder(Size rectangleSize, List<Point> points,
            Tuple<int, int> rangeToRemove, int maxX, int maxY)
        {
            var pointsToInsert = new List<Point>();
            var leftIndex = rangeToRemove.Item1;
            var rightIndex = rangeToRemove.Item2;

            var pointsHadOneElement = leftIndex == rightIndex && leftIndex == 0;
            if (pointsHadOneElement || rightIndex == points.Count)
                pointsToInsert.Add(new Point(maxX, 0));
            else
                pointsToInsert.Add(new Point(maxX, points[rightIndex].Y));

            return pointsToInsert;
        }


        public static Tuple<int, int> FindPointsRangeToRemove(List<Point> points, int maxX, int maxY)
        {
            var rangeToRemove = new List<int>();

            for (var index = 0; index < points.Count; index++)
            {
                var point = points[index];
                if (point.X <= maxX && point.Y <= maxY)
                    rangeToRemove.Add(index);
            }

            return new Tuple<int, int>(rangeToRemove[0], rangeToRemove[rangeToRemove.Count - 1] + 1);
        }

        public void RemovePointsUnderNewRectangle(Tuple<int, int> rangeToRemove)
        {
            points.RemoveRange(rangeToRemove.Item1, rangeToRemove.Item2 - rangeToRemove.Item1);
        }

        private Point FormatPoint(Point rectangleLocation, Size rectangleSize)
        {
            rectangleLocation = MakeCoordinatesAbsolute(rectangleLocation);
            rectangleLocation = FindUpperLeftPoint(rectangleLocation, rectangleSize);
            rectangleLocation = AddCenterCoordinatesToPoint(rectangleLocation);

            return rectangleLocation;
        }

        private Point FindUpperLeftPoint(Point firstToPlace, Size rectangleSize)
        {
            var deltaX = 0;
            var deltaY = 0;

            if (xCoefficient == 1 && yCoefficient == 1)
                deltaY = rectangleSize.Height;

            if (xCoefficient == -1 && yCoefficient == 1)
            {
                deltaX = -rectangleSize.Width;
                deltaY = rectangleSize.Height;
            }

            if (xCoefficient == -1 && yCoefficient == -1)
                deltaX = -rectangleSize.Width;

            var pointX = firstToPlace.X + deltaX;
            var pointY = firstToPlace.Y + deltaY;

            return new Point(pointX, pointY);
        }

        private Point MakeCoordinatesAbsolute(Point rectangleLocation)
        {
            rectangleLocation.X *= xCoefficient;
            rectangleLocation.Y *= yCoefficient;

            return rectangleLocation;
        }

        private Point AddCenterCoordinatesToPoint(Point rectangleLocation)
        {
            rectangleLocation.X += center.X;
            rectangleLocation.Y += center.Y;

            return rectangleLocation;
        }

        public static bool IsPointOnDirectLine(int x, int y, double direction)
        {
            return y == x * direction;
        }

        public static bool IsPointUnderDirectLine(int x, int y, double direction)
        {
            return y < x * direction;
        }

        private void AddPoint(int x, int y)
        {
            points.Add(new Point(x, y));
        }

        private int PointsNumber()
        {
            return points.Count;
        }
    }
}