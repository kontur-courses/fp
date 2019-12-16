using FailuresProcessing;
using System.Drawing;
using System.Linq;
using TagsCloudGenerator.DTO;
using TagsCloudGenerator.Interfaces;

namespace TagsCloudGenerator.PaintersAndSavers
{
    public class PainterAndSaverWithUserImageSize : IPainterAndSaver
    {
        private readonly IFactory<IPainter> paintersFactory;
        private readonly IFactory<ISaver> saversFactory;
        private readonly ISettings settings;

        public PainterAndSaverWithUserImageSize(
            IFactory<IPainter> paintersFactory,
            IFactory<ISaver> saversFactory,
            ISettings settings)
        {
            this.paintersFactory = paintersFactory;
            this.saversFactory = saversFactory;
            this.settings = settings;
        }

        public Result<None> PaintAndSave(WordDrawingDTO[] layoutedWords, string pathToSave)
        {
            return
                CheckUserImageSize()
                .Then(none => CalculateImageSizeAndChangeRectanglesPositions(layoutedWords))
                .Then(size =>
                    Result.Of(() => new Bitmap(size.Width, size.Height))
                    .RefineError($"Failed to create {nameof(Bitmap)} with sizes {size}")
                    .ThenWithDisposableObject(bitmap =>
                        Result.Ok()
                        .Then(none =>
                            paintersFactory.CreateSingle()
                            .Then(painter =>
                                Result.Of(() => Graphics.FromImage(bitmap))
                                .ThenWithDisposableObject(graphics => painter.DrawWords(layoutedWords, graphics))))
                        .Then(none =>
                            saversFactory.CreateSingle()
                            .Then(saver =>
                                Result.Of(() => new Bitmap(bitmap, settings.ImageSize ?? size))
                                .RefineError($"Failed to create {nameof(Bitmap)} with sizes {settings.ImageSize ?? size}")
                                .ThenWithDisposableObject(bitmapToSave => saver.SaveTo(pathToSave, bitmapToSave))))))
                .RefineError($"{nameof(PainterAndSaverWithUserImageSize)} failure");
        }

        private Result<None> CheckUserImageSize()
        {
            var userImageSize = settings.ImageSize;
            return
                !userImageSize.HasValue || (userImageSize.Value.Width > 0 && userImageSize.Value.Height > 0) ?
                Result.Ok() :
                Result.Fail<None>($"Invalid image size: {userImageSize.Value}");
        }

        private Result<Size> CalculateImageSizeAndChangeRectanglesPositions(WordDrawingDTO[] layoutedWords)
        {
            const int frameWidth = 100;
            if (layoutedWords.Length == 0)
                return Result.Ok(new Size(frameWidth * 2, frameWidth * 2));

            var minX = layoutedWords.Min(r => r.WordRectangle.Left);
            var minY = layoutedWords.Min(r => r.WordRectangle.Top);
            var maxX = layoutedWords.Max(r => r.WordRectangle.Right);
            var maxY = layoutedWords.Max(r => r.WordRectangle.Bottom);

            var lengthByX = maxX - minX + 1;
            var lengthByY = maxY - minY + 1;

            var imageSize = new Size((int)lengthByX + 2 * frameWidth, (int)lengthByY + 2 * frameWidth);

            for (var i = 0; i < layoutedWords.Length; i++)
            {
                var newLocation = new PointF(
                    x: layoutedWords[i].WordRectangle.X - minX + frameWidth,
                    y: layoutedWords[i].WordRectangle.Y - minY + frameWidth);
                layoutedWords[i].WordRectangle = new RectangleF(newLocation, layoutedWords[i].WordRectangle.Size);
            }

            return Result.Ok(imageSize);
        }
    }
}