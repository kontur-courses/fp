using ResultOf;
using System.Drawing;
using TagsCloudContainer.SettingsClasses;

namespace TagsCloudContainer.Drawer
{
    public class Painter : IDisposable
    {
        private Graphics graphics;
        public static Result<Image> Draw(IEnumerable<Result<TextImage>> textImages)
        {
            var size = SettingsStorage.AppSettings.DrawingSettings.Size;
            var bgColor = SettingsStorage.AppSettings.DrawingSettings.BgColor;

            var image = new Bitmap(size.Width, size.Height);
            var graphics = Graphics.FromImage(image);

            graphics.Clear(bgColor);

            foreach (var textImage in textImages)
            {
                if (textImage.IsSuccess)
                {
                    graphics.DrawString(textImage.Value.Text, textImage.Value.Font, new SolidBrush(textImage.Value.Color), textImage.Value.Position);
                }
                else
                {
                    return Result<Image>.Fail(textImage.Error);
                }
            }

            return Result<Image>.Ok(image);
        }

        public void Dispose()
        {
            graphics?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}