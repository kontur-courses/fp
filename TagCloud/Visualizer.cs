using System.Drawing;
using TagCloud.Templates;

namespace TagCloud;

internal class Visualizer : IVisualizer
{
    public Result<Bitmap> Draw(ITemplate template)
    {
        var bitmap = new Bitmap(template.ImageSize.Width, template.ImageSize.Height);
        var offset = new PointF(bitmap.Width / 2f, bitmap.Height / 2f);
        using var graphics = Graphics.FromImage(bitmap);
        graphics.FillRectangle(new SolidBrush(template.BackgroundColor), 0, 0, bitmap.Width, bitmap.Height);
        foreach (var wordParameter in template.GetWordParameters())
        {
            var rectangleF = wordParameter.WordRectangleF;
            rectangleF.Offset(offset);
            graphics.DrawString(wordParameter.Word, wordParameter.Font, new SolidBrush(wordParameter.Color),
                rectangleF);
        }

        return bitmap.AsResult();
    }
}