using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization
{
    public class ImageGenerator : IImageGenerator
    {
        private readonly Font font;
        private readonly Bitmap image;
        private readonly Pen pen;

        public ImageGenerator(Bitmap image, Pen pen, Font font)
        {
            this.image = image ?? throw new ArgumentException("Image was null");
            this.pen = pen ?? throw new ArgumentException("Pen was null");
            this.font = font ?? throw new ArgumentException("Font was null");
        }

        public Result<Bitmap> DrawTagCloudBitmap(IEnumerable<ITag> tags)
        {
            if (tags == null)
                return Result.Fail<Bitmap>("Tags was null");

            var tagsList = tags.ToList();
            var graphics = Graphics.FromImage(image);

            foreach (var tag in tagsList)
            {
                graphics.DrawRectangle(pen, tag.Rectangle);
                graphics.DrawString(tag.Word, font, new SolidBrush(pen.Color), tag.Rectangle.Location);
            }

            return image;
        }
    }
}