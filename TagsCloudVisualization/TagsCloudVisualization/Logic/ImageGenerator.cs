using System.Collections.Generic;
using System.Drawing;
using ErrorHandler;
using TagsCloudVisualization.Services;

namespace TagsCloudVisualization.Logic
{
    public class ImageGenerator : IImageGenerator
    {
        private readonly IImageSettingsProvider settingsProvider;

        public ImageGenerator(IImageSettingsProvider settingsProvider)
        {
            this.settingsProvider = settingsProvider;
        }

        public Result<Bitmap> CreateImage(IEnumerable<Tag> tags)
        {
            var imageCenter = new Point(
                settingsProvider.ImageSettings.ImageSize.Width / 2,
                settingsProvider.ImageSettings.ImageSize.Height / 2
            );
            return CalculateCloudScale(tags, imageCenter)
                .Then(cloudScale => DrawTagsAndBackgroundToBitmap(tags, cloudScale, imageCenter));
        }

        private Bitmap DrawTagsAndBackgroundToBitmap(IEnumerable<Tag> tags, float cloudScale, Point imageCenter)
        {
            var bmp = new Bitmap(
                settingsProvider.ImageSettings.ImageSize.Width,
                settingsProvider.ImageSettings.ImageSize.Height
            );
            var graphics = Graphics.FromImage(bmp);
            graphics.FillRectangle(
                new SolidBrush(settingsProvider.ImageSettings.BackgroundColor),
                new Rectangle(Point.Empty, settingsProvider.ImageSettings.ImageSize)
            );
            foreach (var tag in tags)
                DrawTag(graphics, tag, cloudScale, imageCenter);
            return bmp;
        }

        private Result<float> CalculateCloudScale(IEnumerable<Tag> tags, Point imageCenter)
        {
            var cloudScale = 1f;
            foreach (var tag in tags)
            {
                var currentCloudResult = CalculateTagDistanceFromPointScale(tag, imageCenter);
                if (!currentCloudResult.IsSuccess)
                    return currentCloudResult;
                var currentCloudScale = currentCloudResult.GetValueOrThrow();
                if (currentCloudScale < cloudScale)
                    cloudScale = currentCloudScale;
            }
            return cloudScale;
        }

        private Result<float> CalculateTagDistanceFromPointScale(Tag tag, Point imageCenter)
        {
            var cloudBorderSize = settingsProvider.ImageSettings.ImageSize;
            return GetFurthestRectanglePoint(imageCenter, tag.TagBox)
                .Then(furthestRectanglePoint => Geometry
                    .GetLengthFromRectangleCenterToBorderOnVector(
                        new Rectangle(Point.Empty, cloudBorderSize),
                        furthestRectanglePoint
                    )
                    .Then(distanceToImageBorder => distanceToImageBorder == 0 ? 
                        1
                        : (float)(distanceToImageBorder / furthestRectanglePoint.GetLength()))
                );
        }

        private Result<Point> GetFurthestRectanglePoint(Point from, Rectangle rectangle)
        {
            var realFigureCenter = new Point(rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height / 2);
            var posMinusFrom = new Point(realFigureCenter.X - from.X, realFigureCenter.Y - from.Y);
            return Geometry
                .GetLengthFromRectangleCenterToBorderOnVector(
                    new Rectangle(Point.Empty, rectangle.Size),
                    posMinusFrom
                )
                .Then(distanceToBorder =>
                    {
                        var posMinusFromLength = posMinusFrom.GetLength();
                        if (posMinusFromLength == 0)
                            return posMinusFrom;
                        var borderScale = (distanceToBorder + posMinusFromLength) / posMinusFromLength;
                        var posMinusCenterWithTagBorder = new Point(
                            (int)(posMinusFrom.X * borderScale),
                            (int)(posMinusFrom.Y * borderScale)
                        );
                        return posMinusCenterWithTagBorder;
                    }
                );
        }

        private void DrawTag(Graphics graphics, Tag tag, float cloudScale, Point imageCenter)
        {
            var scaledFontSize = tag.FontSize * cloudScale;
            var scaledFont = new Font(
                settingsProvider.ImageSettings.Font.FontFamily,
                scaledFontSize,
                settingsProvider.ImageSettings.Font.Style
            );
            var positionMinusCenter = new Point(
                tag.TagBox.Location.X - imageCenter.X,
                tag.TagBox.Location.Y - imageCenter.Y
            );
            var newPointLocation = new Point(
                imageCenter.X + (int) (positionMinusCenter.X * cloudScale),
                imageCenter.Y + (int) (positionMinusCenter.Y * cloudScale)
            );
            graphics.DrawString(tag.WordToken.Word, scaledFont, new SolidBrush(tag.Color), newPointLocation);
        }
    }
}