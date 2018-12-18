using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TagsCloudContainer.CircularCloudLayouters;
using TagsCloudContainer.Settings;
using TagsCloudContainer.WordsHandlers;

namespace TagsCloudContainer.ImageCreators
{
    public class ImageCreator : IImageCreator
    {
        private readonly IImageSettings imageSettings;
        private readonly ITextSettings textSettings;
        private readonly IPalette palette;
        private readonly Func<ICircularCloudLayouter> circularCloudLayouterFactory;


        public ImageCreator(IImageSettings imageSettings, ITextSettings textSettings, Func<ICircularCloudLayouter> circularCloudLayouterFactory, IPalette palette)
        {
            this.imageSettings = imageSettings;
            this.textSettings = textSettings;
            this.circularCloudLayouterFactory = circularCloudLayouterFactory;
            this.palette = palette;
        }

        public Result<Image> GetImage(IEnumerable<WordInfo> wordInfos)
        {
            var labels = GetDrawingLabels(wordInfos);
            return Result.Of(GetBitmap) 
                .Then(x => DrawLabelsOnBitmap(x, labels))
                .Then(x => (Image)x);
        }

        private Result<Bitmap> DrawLabelsOnBitmap(Bitmap bitmap, IEnumerable<Label> labels)
        {
            var circularCloudLayouter = circularCloudLayouterFactory.Invoke();
            var resultBitmap = bitmap.AsResult();
            foreach (var label in labels)
                resultBitmap = resultBitmap.Then(x => DrawLabelOnBitmap(x, label, circularCloudLayouter));

            return resultBitmap;
        }

        private Result<Bitmap> DrawLabelOnBitmap(Bitmap bitmap, Label label,
            ICircularCloudLayouter circularCloudLayouter)
        {
            return circularCloudLayouter
                .PutNextRectangle(label.Size)
                .Then(x => DrawLabelOnBitmap(bitmap, label, x));
        }

        private Bitmap DrawLabelOnBitmap(Bitmap bitmap, Label label, Rectangle rectangle)
        {
            label.DrawToBitmap(bitmap, rectangle);
            return bitmap;
        }

        private IEnumerable<Label> GetDrawingLabels(IEnumerable<WordInfo> wordInfos)
        {
            var words = textSettings.FontSizeChooser.GetWordInfos(wordInfos);
            var bitmap = GetBitmap();
            var graphics = Graphics.FromImage(bitmap);

            foreach (var printedWordInfo in words)
            {
                var font = new Font(textSettings.Family, printedWordInfo.FontSize);
                var sizeText = graphics.MeasureString(printedWordInfo.Word, font).ToSize();
                var sizeLabel = new Size(sizeText.Width + 20, sizeText.Height + 20);
                var label = new Label
                {
                    Text = printedWordInfo.Word,
                    ForeColor = textSettings.ColorChooser.GetNextColor(),
                    Font = font,
                    Size = sizeLabel,
                    BackColor = palette.BackGroundColor
                };
                label.Update();
                yield return label;
            }
        }

        private Bitmap GetBitmap()
        {
            var size = imageSettings.ImageSize;
            var bitmap = new Bitmap(size.Width, size.Height);
            using (var graphics = Graphics.FromImage(bitmap))
                graphics.Clear(palette.BackGroundColor);
            return bitmap;
        }
    }
}