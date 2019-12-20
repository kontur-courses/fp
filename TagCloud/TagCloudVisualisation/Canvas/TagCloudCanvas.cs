using ResultLogic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace TagCloud.TagCloudVisualisation.Canvas
{
    public class TagCloudCanvas : Canvas
    {
        public TagCloudCanvas(int width, int height) : base(width, height) {}

        public override Result<None> Draw(Rectangle rectangle, Brush brush)
        {
            return Result.OfAction(() => Graphics.FillRectangle(brush, rectangle));
        }

        public override Result<None> Draw(string word, Font font, RectangleF rectangleF, Brush brush)
        {
            return Result.OfAction(() => Graphics.DrawString(word, font, brush, rectangleF));
        }

        public void Save(string fileName)
        {
            Bitmap.Save(fileName + ".png");
        }

        public override Result<None> Save(string directoryPath, string fileName)
        {
            var pathToFile = Path.Combine(directoryPath, fileName + ".png");
            return Result.OfAction(() => Bitmap.Save(pathToFile, ImageFormat.Png));
        }
    }
}
