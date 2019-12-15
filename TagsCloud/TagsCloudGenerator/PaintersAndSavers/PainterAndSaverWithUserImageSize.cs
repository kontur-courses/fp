using FailuresProcessing;
using System.Drawing;
using System.Linq;
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

        public Result<None> PaintAndSave(
            (string word, float maxFontSymbolWidth, string fontName, RectangleF wordRectangle)[] layoutedWords,
            string pathToSave)
        {
            return
                CheckUserImageSize()
                .Then(none => CalculateImageSizeAndChangeRectanglesPositions(layoutedWords))
                .Then(size =>
                {
                    using (var bitmap = new Bitmap(size.Width, size.Height))
                    using (var graphics = Graphics.FromImage(bitmap))
                        return
                            Result.Ok()
                            .Then(none =>
                                paintersFactory.CreateSingle()
                                .Then(painter => painter.DrawWords(layoutedWords, graphics)))
                            .Then(none =>
                                saversFactory.CreateSingle()
                                .Then(saver =>
                                {
                                    using (var bitmapToSave = new Bitmap(bitmap, settings.ImageSize ?? size))
                                        return saver.SaveTo(pathToSave, bitmapToSave);
                                })) ;
                })
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

        private Result<Size> CalculateImageSizeAndChangeRectanglesPositions(
            (string word, float maxFontSymbolWidth, string fontName, RectangleF wordRectangle)[] layoutedWords)
        {
            const int frameWidth = 100;
            if (layoutedWords.Length == 0)
                return Result.Ok(new Size(frameWidth * 2, frameWidth * 2));

            var minX = layoutedWords.Min(r => r.wordRectangle.Left);
            var minY = layoutedWords.Min(r => r.wordRectangle.Top);
            var maxX = layoutedWords.Max(r => r.wordRectangle.Right);
            var maxY = layoutedWords.Max(r => r.wordRectangle.Bottom);

            var lengthByX = maxX - minX + 1;
            var lengthByY = maxY - minY + 1;

            var imageSize = new Size((int)lengthByX + 2 * frameWidth, (int)lengthByY + 2 * frameWidth);

            for (var i = 0; i < layoutedWords.Length; i++)
                layoutedWords[i].wordRectangle.Location = new PointF(
                    x: layoutedWords[i].wordRectangle.X - minX + frameWidth,
                    y: layoutedWords[i].wordRectangle.Y - minY + frameWidth);

            return Result.Ok(imageSize);
        }
    }
}