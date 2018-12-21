using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using ResultOf;

namespace TagCloud
{
    public class CloudVisualizer : ICloudVisualizer
    {
        public string Path { get; }
        private List<WordVisualization> Words { get; } = new List<WordVisualization>();

        public CloudVisualizer(string path)
        {
            Path = path;
        }

        public void AddWord(Word word, Rectangle position, Font font)
        {
            Words.Add(new WordVisualization(word.Value, position, font));
        }

        public Result<Bitmap> CreateImage(Color textColor, Color backgroundColor, Size? size = null)
        {
            var rectangles = Words.Select(w => w.Position).ToList();
            var width = rectangles.Max(r => r.Right) - rectangles.Min(r => r.Left);
            var height = rectangles.Max(r => r.Bottom) - rectangles.Min(r => r.Top);
            
            var imageSize = size ?? new Size(width * 2, height * 2);
            if (imageSize.Width < width || imageSize.Height < height)
                return Result.Fail<Bitmap>("Tag cloud does not fit into the image of the provided size");
            
            var image = new Bitmap(imageSize.Width, imageSize.Height);
            var graphics = Graphics.FromImage(image);
            var newCenter = new Point(image.Width / 2, image.Height / 2);

            graphics.Clear(backgroundColor);
            foreach (var word in Words)
            {
                var x = word.Position.X + newCenter.X;
                var y = word.Position.Y + newCenter.Y;
                graphics.DrawString(word.Value, word.Font, new SolidBrush(textColor), x, y);
            }

            image.Save(Path, ImageFormat.Png);
            return image;
        }
    }
}
