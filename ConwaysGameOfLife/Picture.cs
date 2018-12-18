using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloud
{
    public class Picture : IGraphics
    {
        private readonly ImageSettings imageSettings;

        public Picture(ImageSettings imageSettings)
        {
            this.imageSettings = imageSettings;
        }

        public void Save(IReadOnlyCollection<Tag> words)
        {
            using (var map = new Bitmap(imageSettings.ImageSize.Width, imageSettings.ImageSize.Height))
            using (var graphics = Graphics.FromImage(map))
            {
                var image = new PictureCreator( words, graphics, imageSettings);
                image.DrawPicture();
                map.Save($"{imageSettings.ImageName}", imageSettings.ImageFormat);
            }
        }
    }
}