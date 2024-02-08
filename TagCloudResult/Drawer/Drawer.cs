using System.Drawing;
using TagCloudResult.Layouter;

namespace TagCloudResult.Drawer
{
    public class Drawer(Settings settings, IRectanglesGenerator rectanglesGenerator) : IDrawer
    {
        private Result<Color> GetColorFromName(string name)
        {
            var colorResult = Result.Of(() => Color.FromName(name));
            return !colorResult.IsSuccess
                ? Result.Fail<Color>($"No such color")
                : Result.Ok(colorResult.Value);
        }

        public Result<Image> GetImage()
        {
            var backColorResult = GetColorFromName(settings.BackColor)
                .RefineError("Wrong background color");
            if (!backColorResult.IsSuccess)
                return Result.Fail<Image>(backColorResult.Error);
            var colorResult = GetColorFromName(settings.TextColor)
                .RefineError("Wrong text color");
            if (!colorResult.IsSuccess)
                return Result.Fail<Image>(colorResult.Error);
            var rectanglesDataResult = rectanglesGenerator.GetRectanglesData();
            if (!rectanglesDataResult.IsSuccess)
                return Result.Fail<Image>(rectanglesDataResult.Error);

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
