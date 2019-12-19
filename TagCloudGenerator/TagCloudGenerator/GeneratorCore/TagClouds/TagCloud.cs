using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using TagCloudGenerator.GeneratorCore.CloudLayouters;
using TagCloudGenerator.GeneratorCore.Tags;
using TagCloudGenerator.ResultPattern;

namespace TagCloudGenerator.GeneratorCore.TagClouds
{
    public abstract class TagCloud<TTagType> : ITagCloud where TTagType : Enum
    {
        private readonly Color backgroundColor;

        protected TagCloud(Color backgroundColor, Dictionary<TTagType, TagStyle> tagStyleByTagType)
        {
            this.backgroundColor = backgroundColor;
            TagStyleByTagType = tagStyleByTagType;
        }

        protected Dictionary<TTagType, TagStyle> TagStyleByTagType { get; }

        public Result<Bitmap> CreateBitmap(string[] cloudStrings, ICloudLayouter cloudLayouter, Size bitmapSize)
        {
            var bitmap = new Bitmap(bitmapSize.Width, bitmapSize.Height);
            using var graphics = GetGraphics(bitmap);
            using var backgroundBrush = new SolidBrush(backgroundColor);

            graphics.FillRectangle(backgroundBrush, new Rectangle(Point.Empty, bitmap.Size));

            var tagDrawer = GetTagDrawer(graphics);
            var canvas = new Rectangle(Point.Empty, bitmapSize);

            foreach (var tag in GetTags(cloudStrings, graphics, cloudLayouter))
                if (tag.IsSuccess)
                {
                    if (!canvas.Contains(tag.Value.TagBox))
                        return Result.Fail<Bitmap>($"Tag {tag.Value.TagBox} is out of canvas.");

                    tagDrawer(tag.Value);
                }
                else
                    return Result.Fail<Bitmap>($"Tag error was handled:{Environment.NewLine}{tag.Error}");

            return bitmap.AsResult();
        }

        protected abstract Action<Tag> GetTagDrawer(Graphics graphics);

        protected abstract IEnumerable<Result<Tag>> GetTags(string[] cloudStrings,
                                                            Graphics graphics,
                                                            ICloudLayouter circularCloudLayouter);

        protected static Brush GetBrush(Color color, Dictionary<Color, Brush> brushByColor)
        {
            if (brushByColor.TryGetValue(color, out var brush))
                return brush;
            return brushByColor[color] = new SolidBrush(color);
        }

        private static Graphics GetGraphics(Image tagCloudImage)
        {
            var graphics = Graphics.FromImage(tagCloudImage);

            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

            return graphics;
        }
    }
}