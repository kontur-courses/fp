using System.Collections.Generic;
using System.Drawing;

namespace TagsCloud
{
    public class PictureCreator
    {
        private readonly Graphics graphics;
        private readonly ImageSettings imageSettings;
        private readonly IReadOnlyCollection<Tag> tags;

        public PictureCreator(IReadOnlyCollection<Tag> tags, Graphics graphics, ImageSettings imageSettings)
        {
           this.tags = tags;
            this.graphics = graphics;
            this.imageSettings = imageSettings;
        }

        public void DrawPicture()
        {
            var drawFormat = new StringFormat
            {
                FormatFlags = StringFormatFlags.DirectionRightToLeft,
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            graphics.FillRectangle(new SolidBrush(Color.White), new Rectangle(0, 0, imageSettings.ImageSize.Width, imageSettings.ImageSize.Height));
            foreach (var tag in tags)
                using (var brush = new SolidBrush(imageSettings.Color))
                {
                    var currentFont = new Font(imageSettings.FontFamily, tag.WordBox.Height);
                    graphics.DrawString(tag.Word, currentFont, brush, tag.WordBox, drawFormat);
                }
        }
    }
}