#region

using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization.Interfaces;

#endregion

namespace TagsCloudVisualization
{
    public class ImageGenerator : IImageGenerator
    {
        private readonly Font font;
        private readonly Bitmap image;
        private readonly Pen pen;

        public ImageGenerator(Bitmap image, Pen pen, Font font)
        {
            this.image = image;
            this.pen = pen;
            this.font = font;
        }

        public Result<Graphics> GetGraphics()
        {
            return image == null
                ? new Result<Graphics>("Image was null")
                : new Result<Graphics>(null, Graphics.FromImage(image));
        }

        public Result<Font> GetFont()
        {
            return font == null
                ? new Result<Font>("Font was null")
                : new Result<Font>(null, font);
        }

        public Result<Bitmap> DrawTagCloudBitmap(IEnumerable<ITag> tags)
        {
            if (tags == null) return new Result<Bitmap>("Tags was null");

            var tagsList = tags.ToList();
            var brush = Graphics.FromImage(image);

            foreach (var tag in tagsList)
            {
                brush.DrawRectangle(pen, tag.Rectangle);
                brush.DrawString(tag.Word, font, new SolidBrush(pen.Color), tag.Rectangle.Location);
            }

            return new Result<Bitmap>(null, image);
        }
    }
}