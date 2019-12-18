using System.Collections.Generic;
using System.Drawing;
using TagsCloudResult.Infrastructure.Common;

namespace TagsCloudResult
{
    public static class CloudDrawer
    {
        public static Result<Bitmap> Draw(IEnumerable<(Rectangle, LayoutWord)> words, AppSettings appSetting)
        {
            var setting = appSetting.ImageSetting;   
            if (setting.Width < 0 || setting.Height < 0)
                return Result.Fail<Bitmap>("Width or Height is not validate");
            
            var color = Color.FromName(setting.BackGround);
            if (!color.IsKnownColor)
                return Result.Fail<Bitmap>($"Unresolved color {setting.BackGround}");
            
            var bitmap = new Bitmap(setting.Width, setting.Height);
            var graphic = Graphics.FromImage(bitmap);
            graphic.FillRectangle(new SolidBrush(color),
                new Rectangle(0, 0, setting.Width, setting.Height));
            foreach (var (rectangle, layoutWord) in words)
            {
                var locationShift = rectangle.Location + new Size(setting.Width / 2, setting.Height / 2);
                var rectangleShift = new Rectangle(locationShift, rectangle.Size);
                graphic.DrawString(layoutWord.Word, layoutWord.Font, layoutWord.Brush, rectangleShift);
            }

            return bitmap;
        }
    }
}