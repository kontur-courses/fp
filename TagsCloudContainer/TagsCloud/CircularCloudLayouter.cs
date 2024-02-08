using System.Drawing;
using TagsCloudContainer.Extensions;
using TagsCloudContainer.Interfaces;
using TagsCloudContainer.Utility;

namespace TagsCloudContainer.TagsCloud
{
    public class CircularCloudLayouter : ICircularCloudLayouter
    {
        private readonly INextPointProvider pointProvider;

        public Result<Point> CloudCenter { get; init; } = Point.Empty;
        public IList<Rectangle> Rectangles { get; init; } = new List<Rectangle>();

        public CircularCloudLayouter(Result<Point> center, INextPointProvider pointProvider)
        {
            CloudCenter = center;
            this.pointProvider = pointProvider;
        }

        private const int MinPositiveValue = 1;

        public Result<Rectangle> PutNextRectangle(Size rectangleSize)
        {
            try
            {
                ValidateRectangleSize(rectangleSize);

                var currentRectangleResult = CreateNewRectangle(rectangleSize);

                if (currentRectangleResult.IsSuccess)
                {
                    var currentRectangle = currentRectangleResult.Value;
                    Rectangles.Add(currentRectangle);
                    return Result.Ok(currentRectangle);
                }
                else
                {
                    return Result.Fail<Rectangle>(currentRectangleResult.Error);
                }
            }
            catch (Exception ex)
            {
                return Result.Fail<Rectangle>($"Error putting next rectangle: {ex.Message}");
            }
        }

        public Result<Rectangle> PutNextRectangle(string word, Font font)
        {
            try
            {
                var textSizeResult = MeasureTextSize(word, font);

                if (textSizeResult.IsSuccess)
                {
                    var textSize = textSizeResult.Value;
                    return PutNextRectangle(textSize);
                }
                else
                {
                    return Result.Fail<Rectangle>($"Error putting next rectangle for word '{word}': {textSizeResult.Error}");
                }
            }
            catch (Exception ex)
            {
                return Result.Fail<Rectangle>($"Error putting next rectangle for word '{word}': {ex.Message}");
            }
        }

        private Result<Size> MeasureTextSize(string text, Font font)
        {
            try
            {
                // размер минимального временного изображения для измерения текста
                var imageSizeForTextMeasurement = new Size(MinPositiveValue, MinPositiveValue);

                // временное изображение с заданным размером
                using (var temporaryBitmap = new Bitmap(imageSizeForTextMeasurement.Width, imageSizeForTextMeasurement.Height))
                {
                    using (var temporaryGraphics = Graphics.FromImage(temporaryBitmap))
                    {
                        var textSize = Size.Ceiling(temporaryGraphics.MeasureString(text, font));

                        return Result.Ok(textSize);
                    }
                }
            }
            catch (Exception ex)
            {
                return Result.Fail<Size>($"Error measuring text size: {ex.Message}");
            }
        }

        private void ValidateRectangleSize(Size rectangleSize)
        {
            rectangleSize.ValidateSize(MinPositiveValue);
        }

        private Result<Rectangle> CreateNewRectangle(Size rectangleSize)
        {
            while (true)
            {
                var nextPointResult = pointProvider.GetNextPoint();

                if (nextPointResult.IsSuccess)
                {
                    var nextPoint = nextPointResult.Value;
                    var rectangleLocation = GetUpperLeftCorner(nextPoint, rectangleSize);
                    var rectangle = new Rectangle(rectangleLocation, rectangleSize);

                    if (!RectanglesIntersect(rectangle))
                    {
                        return Result.Ok(rectangle);
                    }
                }
                else
                {
                    return Result.Fail<Rectangle>($"Error getting next point for rectangle: {nextPointResult.Error}");
                }
            }
        }

        private Point GetUpperLeftCorner(Point rectangleCenter, Size rectangleSize)
        {
            var center = TagCloudApp.CalculateCenter(rectangleSize.Width, rectangleSize.Height);
            return new Point(rectangleCenter.X - center.X, rectangleCenter.Y - center.Y);
        }

        private bool RectanglesIntersect(Rectangle newRectangle)
        {
            return Rectangles.Any(rect => rect.IntersectsWith(newRectangle));
        }

    }
}
