using System.Collections.Generic;
using System.Drawing;
using TagsCloudResult.Infrastructure.Common;

namespace TagsCloudResult
{
    public static class CloudDrawer
    {
        public static Result<Bitmap> Draw(IEnumerable<(Rectangle, LayoutWord)> words, ImageSetting setting)
        {
            var bitmap = new Bitmap(setting.Width, setting.Height);
            var graphic = Graphics.FromImage(bitmap);
            graphic.FillRectangle(new SolidBrush(setting.BackGround),
                new Rectangle(0, 0, setting.Width, setting.Height));
            foreach (var (rectangle, layoutWord) in words)
            {
                graphic.DrawString(layoutWord.Word, layoutWord.Font, layoutWord.Brush, rectangle);
            }

            return bitmap;
        }
    }
}