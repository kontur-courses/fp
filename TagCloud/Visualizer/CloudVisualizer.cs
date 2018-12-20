using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using TagCloud.Extensions;
using TagCloud.Layouter;
using TagCloud.Models;
using TagCloud.Visualizer.Drawer;
using TagCloud.Visualizer.Settings;

namespace TagCloud.Visualizer
{
    public class CloudVisualizer : ICloudVisualizer
    {
        public IDrawSettings Settings { get; set; }
        private readonly ICloudDrawer cloudDrawer;
        private readonly ICloudLayouter layouter;
        private readonly Size pictureSize;

        public CloudVisualizer(IDrawSettings settings, ICloudLayouter layouter, ICloudDrawer cloudDrawer, Size pictureSize)
        {
            Settings = settings;
            this.layouter = layouter;
            this.cloudDrawer = cloudDrawer;
            this.pictureSize = pictureSize;
        }

        public Bitmap CreatePictureWithItems(IList<TagItem> items)
        {
            if (items == null)
                throw new ArgumentException("Array can't be null");
            if (items.Count == 0)
                throw new ArgumentException("Array can't be empty");

            var picture = new Bitmap(pictureSize.Width, pictureSize.Height);

            using (var graphics = Graphics.FromImage(picture))
            {
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PageUnit = GraphicsUnit.Pixel;
                graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
                graphics.Clear(Settings.Color);
                graphics.TranslateTransform(pictureSize.Width / 2, pictureSize.Height / 2);

                var cloudItems = new List<CloudItem>();
                foreach (var tagItem in items)
                {
                    var font = new Font(Settings.Font.FontFamily, tagItem.FontSize, Settings.Font.Style);
                    var stringSize = Size.Round(graphics.MeasureString(tagItem.Word, font));
                    var item = new CloudItem(
                        tagItem.Word,
                        layouter.PutNextRectangle(stringSize),
                        font);
                    cloudItems.Add(item);
                }

                CheckResultSize(cloudItems);
                cloudDrawer.Draw(graphics, cloudItems, Settings);
            }
            return picture;
        }

        private void CheckResultSize(IList<CloudItem> items)
        {
            var pictureBound = new Rectangle(
                -pictureSize.Width / 2,
                -pictureSize.Height / 2,
                pictureSize.Width,
                pictureSize.Height);
            if (items.Any(item => !pictureBound.Contains(item.Bounds)))
                throw new ArgumentException($"Tag cloud does not fit the image, need size {GetSuitableSize(items)}");
        }

        private static Size GetSuitableSize(IEnumerable<CloudItem> items)
        {
            var bounds = items
                .SelectMany(item => new[] { item.Bounds.Location, new Point(item.Bounds.Right, item.Bounds.Bottom) })
                .ToArray()
                .GetBounds();
            return new Size(
                Math.Max(bounds.X * -1, bounds.Right) * 2 + 2,
                Math.Max(bounds.Y * -1, bounds.Bottom) * 2 + 2);
        }
    }
}