using System;
using System.Drawing;
using System.Linq;
using ResultOf;

namespace TagsCloudContainer.Cloud
{
    public class TagCloudRenderer
    {
        private TagCloud tagCloud;
        private Result<string> fontName;
        private Result<Brush> brush;


        public TagCloudRenderer(TagCloud tagCloud, Result<string> fontName, Result<Brush> brush)
        {
            this.tagCloud = tagCloud;
            this.fontName = fontName;
            this.brush = brush;
        }

        public Result<Image> GenerateImage()
        {
            return tagCloud.Tags.Then(wordTags =>
            {

                var maxX = wordTags.Max(x => x.DescribedRectangle.X + x.DescribedRectangle.Width);
                var maxY = wordTags.Max(x => x.DescribedRectangle.Y + x.DescribedRectangle.Height);
                var image = new Bitmap(maxX + 50, maxY + 50);
                var graphics = Graphics.FromImage(image);

                foreach (var wordTag in wordTags)
                {
                    var rectangle = wordTag.DescribedRectangle;
                    fontName.Then((n) => ValidateFont(n));
                    ValidateFont(fontName.Value);
                    Font drawFont = new Font(fontName.Value, rectangle.Height / 2);
                    graphics.DrawString(wordTag.Word, drawFont, brush.GetValueOrThrow(), rectangle.X, rectangle.Y);
                }

                return image as Image;
            }).RefineError("Image generate error");
        }

        private Result<string> ValidateFont(string name)
        {
            var fontFamily = new FontFamily(fontName.Value);
            if (FontFamily.Families.Contains(fontFamily))
                return Result.Ok(name);
            return Result.Fail<string>("Font not found");
        }
    }
}
