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
                    var drawFont = CreateFont(fontName, rectangle.Height);
                    if (!drawFont.IsSuccess)
                        return Result.Fail<Image>(fontName.Error);
                    graphics.DrawString(wordTag.Word, drawFont.Value, brush.Value, rectangle.X, rectangle.Y);
                }

                return image as Image;
            }).RefineError("Image generate error");
        }

        private Result<Font> CreateFont(Result<string> fontName, int height)
        {
            return fontName.Then((name) =>
            {
                var fontFamily = new FontFamily(fontName.Value);
                if (FontFamily.Families.Contains(fontFamily))
                    return Result.Ok(new Font(name, height / 2));
                return Result.Fail<Font>("Font not found");
            });
        }
    }
}
