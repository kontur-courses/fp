using System.Collections.Generic;
using System.Drawing;
using System.IO;
using CSharpFunctionalExtensions;
using TagsCloudContainer.Layout;
using TagsCloudContainer.Settings;

namespace TagsCloudContainer.Drawing
{
    public class ImageDrawer : IDrawer
    {
        public Result<byte[]> Draw(HashSet<Tag> tags, ImageSettings settings)
        {
            using (var bitmap = new Bitmap(settings.Size.Width, settings.Size.Height))
            {
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    graphics.FillRectangle(
                        new SolidBrush(settings.BackgroundColor),
                        new Rectangle(new Point(0, 0), settings.Size));

                    foreach (var tag in tags)
                    {
                        graphics.DrawRectangle(new Pen(settings.RectangleColor), tag.ContainingRectangle);
                        graphics.DrawString(tag.Word, tag.Font, new SolidBrush(settings.TextColor), tag.ContainingRectangle);
                    }

                    using (var stream = new MemoryStream())
                    {
                        bitmap.Save(stream, settings.ImageFormat);
                        return Result.Ok(stream.ToArray());
                    }
                }
            }
        }
    }
}