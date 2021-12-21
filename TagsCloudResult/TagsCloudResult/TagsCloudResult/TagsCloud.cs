using ResultOf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization;

namespace TagsCloudContainer
{
    public static class TagsCloud
    {
        public static Result<IEnumerable<Word>> CreateCloud<T>(Dictionary<T, int> words, ImageSettings setting)
        {
            var cloudCenter = new Point();
            var circularCloudLayouter = new CircularCloudLayouter(cloudCenter);
            var graphic = Graphics.FromImage(new Bitmap(setting.ImageSize.Width, setting.ImageSize.Height));
            var cloudCreator = new WordCloudCreatorFunc<string>(circularCloudLayouter.PutNextRectangle, graphic.MeasureString);
            var wordsCloud = cloudCreator.CreateWordCloud(words, setting).ToList();
            var imageBorder = new Rectangle(new Point(), setting.ImageSize);

            if (wordsCloud.Select(w => w.Border).Where(r => !imageBorder.Contains(r)).Any())
                return Result.Fail<IEnumerable<Word>>("Облако тегов не влезло на изображение заданного размера");

            return Result.Ok((IEnumerable<Word>)wordsCloud);
        }

        public static Result<Bitmap> PaintWords<T>(Func<Dictionary<T, int>, ImageSettings, Result<IEnumerable<Word>>> getWords, Dictionary<T, int> words, Func<Result<ImageSettings>> getImageSettings)
        {
            var resBitmap = getImageSettings().Then(settings =>
            {
                var imageSize = settings.ImageSize;
                var bitmap = new Bitmap(imageSize.Width, imageSize.Height);

                var graphic = Graphics.FromImage(bitmap);
                graphic.Clear(settings.BackgroundColor);

                var cloudRes = getWords(words, settings).Then(words =>
                {
                    foreach (var word in words)
                        graphic.DrawString(word.Text, word.Font, new SolidBrush(settings.TextColor), word.Border);
                });

                if (!cloudRes.IsSuccess)
                    return Result.Fail<Bitmap>(cloudRes.Error);

                return bitmap;
            });

            return resBitmap;
        }
    }
}
