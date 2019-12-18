using SyntaxTextParser;
using System.Drawing;
using ResultPattern;
using TagsCloudGenerator.CloudPrepossessing;

namespace TagsCloudGenerator
{
    public class BitmapBaseCloudBuilder : CloudBuilder<Bitmap>
    {
        public BitmapBaseCloudBuilder(TextParser parser, ITagsPrepossessing tagPlacer) 
            : base(parser, tagPlacer)
        { }

        public override Result<Bitmap> CreateTagCloudRepresentation(string fullPath, Size imageSize, CloudFormat format)
        {
            var tags = TagGenerator.CreateCloudTags(fullPath, Parser, TagPlacer, format);
            if (!tags.IsSuccess)
                return Result.Fail<Bitmap>(tags.Error);

            var bitmap = new Bitmap(imageSize.Width, imageSize.Height);
            var graphics = Graphics.FromImage(bitmap);
            graphics.Clear(format.ColorPainter.BackgroundColor);

            var textPen = new Pen(Color.Black);
            var rectPen = new Pen(Color.Black);

            foreach (var tag in tags.GetValueOrThrow())
            {
                rectPen.Color = format.ColorPainter.GetTagShapeColor();
                graphics.DrawRectangle(rectPen, tag.Shape);

                textPen.Color = format.ColorPainter.GetTagTextColor(rectPen.Color);
                var text = format.TagTextPreform.PreformToVisualize(tag.Text);

                graphics.DrawString(text, tag.TextFont, textPen.Brush,
                    tag.Shape.ConvertToRectangleF(), tag.Format);
            }

            return bitmap.AsResult();
        }
    }
}