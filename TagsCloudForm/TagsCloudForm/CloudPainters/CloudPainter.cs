﻿using System;
using System.Drawing;
using CircularCloudLayouter;
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

        public CloudPainter(IImageHolder imageHolder,
            CircularCloudLayouterSettings.ICircularCloudLayouterSettings settings, IPalette palette, ICircularCloudLayouter layouter)
        {
            this.imageHolder = imageHolder;
            this.settings = settings;
            this.palette = palette;
            this.layouter = layouter;
            imageSize = imageHolder.GetImageSize();
        }

        public void Paint()
        {
            using (var graphics = imageHolder.StartDrawing())
            using (var backgroundBrush = new SolidBrush(palette.SecondaryColor))
            using (var rectBrush = new Pen(palette.PrimaryColor))
            using (var backgroundPictureBrush = new SolidBrush(palette.BackgroundColor))
            {
                graphics.FillRectangle(backgroundPictureBrush, 0, 0, imageSize.Width, imageSize.Height);
                var rnd = new Random();
                for (var i = 0; i < settings.IterationsCount; i++)
                {
                    var rectangleSize = new Size(rnd.Next(settings.MinSize, settings.MaxSize),
                        rnd.Next(settings.MinSize, settings.MaxSize));
                    var rectangle = layouter.PutNextRectangle(rectangleSize);
                    graphics.FillRectangle(backgroundBrush, rectangle);
                    graphics.DrawRectangle(rectBrush, rectangle);
                }
            }
            imageHolder.UpdateUi();
        }
    }
}
