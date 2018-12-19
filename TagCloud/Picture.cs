using System.Collections.Generic;
using System.Drawing;

namespace TagsCloud
{
    public class Picture : IGraphics
    {
        private readonly Result<ImageSettings> imageSettings;

        public Picture(Result<ImageSettings> imageSettings)
        {
            this.imageSettings = imageSettings;
        }

        public Result<None> Save(IReadOnlyCollection<Tag> words)
        {
            return imageSettings
                .Then(settings =>
                {
                    using (var map = new Bitmap(settings.ImageSize.Width, settings.ImageSize.Height))
                    using (var graphics = Graphics.FromImage(map))
                    {
                        var image = new PictureCreator(words, graphics, settings);
                        image.DrawPicture();
                        map.Save($"{settings.ImageName}", settings.ImageFormat);
                    }
                });
        }
    }
}