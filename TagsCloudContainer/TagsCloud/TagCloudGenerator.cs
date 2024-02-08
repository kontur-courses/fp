using System.Drawing;
using TagsCloudContainer.Interfaces;

namespace TagsCloudContainer.TagsCloud
{
    public class TagCloudGenerator : ITagCloudGenerator
    {
        public Bitmap GenerateTagCloud(IEnumerable<string> words, IImageSettings imageSettings)
        {
            var tagCloudImage = new Bitmap(imageSettings.Width, imageSettings.Height);

            using (var graphics = Graphics.FromImage(tagCloudImage))
            using (var brush = new SolidBrush(imageSettings.FontColor))
            {
                graphics.Clear(imageSettings.BackgroundColor);
                Font font = imageSettings.GetFont();

                int OffsetY = 0;
                foreach (var word in words)
                {
                    graphics.DrawString(word, font, brush, new PointF(0, OffsetY));
                    OffsetY += font.Height;
                }
            }

            return tagCloudImage;
        }
    }
}

