using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace TagCloud.TagCloudVisualisation.Canvas
{
    public class TagCloudCanvas : TagCloudVizualisation.Canvas.Canvas
    {
        public TagCloudCanvas(int width, int height) : base(width, height) {}

        public override void Draw(Rectangle rectangle, Brush brush)
        {
            Graphics.FillRectangle(brush, rectangle);
        }

        public override void Draw(string word, Font font, RectangleF rectangleF, Brush brush)
        {
            Graphics.DrawString(word, font, brush, rectangleF);
        }

        public void Save(string fileName)
        {
            Bitmap.Save(fileName + ".png");
        }

        public override void Save(string directoryPath, string fileName)
        {
            var pathToFile = Path.Combine(directoryPath, fileName + ".png");
            Bitmap.Save(pathToFile, ImageFormat.Png);
        }
    }
}
