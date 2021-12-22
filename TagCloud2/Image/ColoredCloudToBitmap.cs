using ResultOf;
using System.Drawing;

namespace TagCloud2.Image
{
    public class ColoredCloudToBitmap : IColoredCloudToImageConverter
    {
        public Result<System.Drawing.Image> GetImage(IColoredCloud cloud, int xSize, int ySize)
        {
            if (xSize < 1 || ySize < 1)
            {
                return Result.Fail<System.Drawing.Image>("Size must be positive");
            }

            var words = cloud.ColoredWords;
            var bitmap = new Bitmap(xSize, ySize);
            var graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.Black);
            foreach (var coloredSizedWord in words)
            {
                if (CheckThatWordInsideBitmap(xSize, ySize, coloredSizedWord))
                {
                    return Result.Fail<System.Drawing.Image>("Cloud is bigger than image");
                }

                var word = coloredSizedWord.Word;
                var brush = new SolidBrush(coloredSizedWord.Color);
                graphics.DrawString(word, coloredSizedWord.Font, brush, coloredSizedWord.Size);
            }

            return bitmap;
        }

        private static bool CheckThatWordInsideBitmap(int xSize, int ySize, TextGeometry.IColoredSizedWord coloredSizedWord)
        {
            return coloredSizedWord.Size.X > xSize || coloredSizedWord.Size.X < 0
                                || coloredSizedWord.Size.Y > ySize || coloredSizedWord.Size.Y < 0;
        }
    }
}
