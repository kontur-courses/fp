using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Functional;

namespace TagCloudVisualization
{
    public class TagCloudImageCreator
    {
        private const double Expander = 1.2;
        private readonly CompositeDrawer compositeDrawer;
        private readonly Func<Point, CircularCloudLayouter> layouterFactory;

        public TagCloudImageCreator(CompositeDrawer drawer, Func<Point, CircularCloudLayouter> layouterFactory)
        {
            compositeDrawer = drawer;
            this.layouterFactory = layouterFactory;
        }

        public float MaxFontSize => 100;

        public virtual Result<Bitmap> CreateTagCloudImage(IEnumerable<WordInfo> tagCloud, ImageCreatingOptions options)
        {
            return SetRectanglesToCloud(tagCloud, options)
                   .Then(p => DrawTagCloud(p, options))
                   .ReplaceError(err => "Can't create image: " + err);
        }

        private Result<Bitmap> DrawTagCloud(IEnumerable<WordInfo> tagCloud, ImageCreatingOptions options)
        {
            tagCloud = tagCloud.ToList();
            var image = CreateImage(tagCloud, options);

            if (!image.IsSuccess)
                return Result.Fail<Bitmap>(image.Error);

            var center = new Point(image.Value.Size.Width / 2, image.Value.Size.Height / 2);

            using (var graphics = Graphics.FromImage(image.Value))
                foreach (var wordInfo in tagCloud)
                {
                    if (wordInfo.Scale.HasNoValue || wordInfo.Rectangle.HasNoValue)
                        return Result.Fail<Bitmap>("WordInfo have not enough values");

                    var rectangle = wordInfo.Rectangle.Value;
                    var fontScale = wordInfo.Scale.Value;

                    using (var font = new Font(options.FontName, fontScale))
                    {
                        rectangle.Offset(center);
                        DrawSingleWord(graphics, options, wordInfo, font);
                    }
                }

            return image;
        }

        private static Result<Bitmap> CreateImage(IEnumerable<WordInfo> tagCloud, ImageCreatingOptions options)
        {
            var (width, height, _) = GetTagCloudDimensions(tagCloud);

            if (options.ImageSize == null)
                return new Bitmap(width, height);

            width = options.ImageSize.Value.Width;
            height = options.ImageSize.Value.Height;

            return new Bitmap(width, height);
        }

        private static (int width, int height, Point center) GetTagCloudDimensions(IEnumerable<WordInfo> tagCloud)
        {
            tagCloud = tagCloud.ToList();
            var areaSize = tagCloud.Select(w => w.Rectangle.Value)
                                   .GetUnitedSize();

            areaSize = new Size((int) (areaSize.Width * Expander), (int) (areaSize.Height * Expander));

            var width = areaSize.Width;
            var height = areaSize.Height;
            var center = new Point(areaSize.Width / 2, areaSize.Height / 2);
            return (width, height, center);
        }

        private Result<IEnumerable<WordInfo>> SetRectanglesToCloud(
            IEnumerable<WordInfo> tagCloud,
            ImageCreatingOptions options)
        {
            var layouter = layouterFactory(options.Center);
            var results = tagCloud
                          .Select(wordInfo => (wordInfo, rectangle: GetRectangleForWord(layouter, wordInfo, options)))
                          .ToList();
            return results.Any(p => !p.rectangle.IsSuccess)
                       ? Result.Fail<IEnumerable<WordInfo>>("Can't add rectangle to Cloud")
                       : results.Select(p => p.wordInfo.With(p.rectangle.Value))
                                .AsResult();
        }

        private static Result<Rectangle> GetRectangleForWord(
            CircularCloudLayouter layouter,
            WordInfo wordInfo,
            ImageCreatingOptions options)
        {
            if (wordInfo.Scale.HasNoValue)
                return Result.Fail<Rectangle>("Scale has no value");
            using (var font = new Font(options.FontName, wordInfo.Scale.Value))
            {
                var size = TextRenderer.MeasureText(wordInfo.Word, font);
                var rectangle = layouter.PutNextRectangle(size);
                return rectangle;
            }
        }

        private protected Result<None> DrawSingleWord(
            Graphics graphics,
            ImageCreatingOptions options,
            WordInfo wordInfo,
            Font font)
        {
            return compositeDrawer.GetDrawer(wordInfo)
                                  .Then(d => d.DrawWord(graphics, options, wordInfo, font))
                                  .RefineError($"There is no drawer that can draw given word: {wordInfo.Word}");
        }
    }
}
