using System;
using System.Drawing;
using System.Threading;

namespace TagsCloudResult.CircularCloudLayouter
{
    internal class RectangleVisualizer: IDrawer<Rectangle>
    {
        private readonly CircularCloudLayout layout;
        private Bitmap bitmap;

        public RectangleVisualizer(CircularCloudLayout layout)
        {
            this.layout = layout;
            CreateBitmapForDrawing(this.layout.ImageSize());
        }

        public void DrawTagsCloud(string resultFilePath)
        {
            var g = Graphics.FromImage(bitmap);
            DrawAllRectangles(g);

            bitmap.Save(resultFilePath);
        }

        private void DrawAllRectangles(Graphics g)
        {
            foreach (var rectangle in layout.GetCoordinatesToDraw())
                g.FillRectangle
                (
                    TakeRandomColor(),
                    rectangle.X, rectangle.Y,
                    rectangle.Size.Width,
                    rectangle.Size.Height
                );
        }

        private static Brush TakeRandomColor()
        {
            var rnd = new Random();
            var color = Color.FromArgb(rnd.Next());

            var brush = new SolidBrush(color);
            Thread.Sleep(25);

            return brush;
        }

        private void CreateBitmapForDrawing(Size imageSize)
        {
            bitmap = new Bitmap(imageSize.Width, imageSize.Height);
        }

        public Result<None> DrawItems(string resultFilePath)
        {
            try
            {
                DrawTagsCloud(resultFilePath);
                return new Result<None>();
            }
            catch (Exception e)
            {
                return new Result<None>(e.Message);
            }
        }
    }
}