using System;
using System.Drawing;
using System.Windows.Forms;
using CircularCloudLayouter;
using OpenNLP.Tools.Trees.TRegex.Tsurgeon;
using TagsCloudForm.CircularCloudLayouterSettings;
using TagsCloudForm.Common;

namespace TagsCloudForm.CloudPainters
{
    public class CloudPainter : ICloudPainter
    {
        private readonly IImageHolder imageHolder;
        private readonly CircularCloudLayouterSettings.ICircularCloudLayouterSettings settings;
        private readonly IPalette palette;
        private Size imageSize;
        private ICircularCloudLayouter layouter;
        private Random rnd;

        public CloudPainter(IImageHolder imageHolder,
            CircularCloudLayouterSettings.ICircularCloudLayouterSettings settings, IPalette palette, ICircularCloudLayouter layouter)
        {
            this.imageHolder = imageHolder;
            this.settings = settings;
            this.palette = palette;
            this.layouter = layouter;
            this.rnd = new Random();
            imageSize = imageHolder.GetImageSize();
        }

        public void Paint()
        {
            using (var graphics = imageHolder.StartDrawing())
            using (var backgroundBrush = new SolidBrush(palette.SecondaryColor))
            using (var rectBrush = new Pen(palette.PrimaryColor))
            using (var backgroundPictureBrush = new SolidBrush(palette.BackgroundColor))
            {
                Result
                    .OfAction(() =>
                    graphics.FillRectangle(backgroundPictureBrush, 0, 0, imageSize.Width, imageSize.Height))
                    .OnError(err => MessageBox.Show(err));
                for (var i = 0; i < settings.IterationsCount; i++)
                {
                    Result
                        .Of(() => GetRandomSize(settings))
                        .Then(x => layouter.PutNextRectangle(x))
                        .Then(x=> DrawRectangle(graphics, backgroundBrush, rectBrush, x))
                        .OnError(err => MessageBox.Show(err));
                }
            }
            imageHolder.UpdateUi();
        }

        private void DrawRectangle(IGraphicDrawer graphics, SolidBrush backgroundBrush, Pen rectPen, Rectangle rectangle)
        {
            graphics.FillRectangle(backgroundBrush, rectangle);
            graphics.DrawRectangle(rectPen, rectangle);
        }

        private Size GetRandomSize(ICircularCloudLayouterSettings settings)
        {
            return new Size(rnd.Next(settings.MinSize, settings.MaxSize),
                rnd.Next(settings.MinSize, settings.MaxSize));
        }
    }
}
