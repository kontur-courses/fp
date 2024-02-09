using System.Drawing;
using TagCloudResult.Layouter;

namespace TagCloudResult.Drawer
{
    public class Drawer(Settings settings, IRectanglesGenerator rectanglesGenerator) : IDrawer
    {
        public Result<Image> GetImage()
        {
            var backColorResult = ResultIs.Of(() => Color.FromName(settings.BackColor), "Wrong background color");
            if (!backColorResult.IsSuccess)
                return backColorResult.Fail<Image>();
            var colorResult = ResultIs.Of(() => Color.FromName(settings.TextColor), "Wrong text color");
            if (!colorResult.IsSuccess)
                return colorResult.Fail<Image>();
            var rectanglesDataResult = rectanglesGenerator.GetRectanglesData();
            if (!rectanglesDataResult.IsSuccess)
                return rectanglesDataResult.Fail<Image>();

            var image = new Bitmap(settings.ImageWidth, settings.ImageHeight);
            using var gr = Graphics.FromImage(image);
            gr.Clear(backColorResult.Value);
            var brush = new SolidBrush(colorResult.Value);
            foreach (var rectangleData in rectanglesDataResult.Value)
                using (var font = new Font(settings.FontName, rectangleData.fontSize, FontStyle.Regular))
                    gr.DrawString(rectangleData.word, font, brush, rectangleData.rectangle);

            return image;
        }
    }
}
