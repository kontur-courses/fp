using ResultOf;
using System.Drawing;

namespace TagsCloudContainer.Drawer
{
    public class Painter : IDisposable
    {
        private Graphics graphics;
        public static Result<Image> Draw(Size size, IEnumerable<Result<TextImage>> textImages, Color? bgColor = null)
        {
            var image = new Bitmap(size.Width, size.Height);
            var graphics = Graphics.FromImage(image);

            graphics.Clear(bgColor ?? Color.Black);

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