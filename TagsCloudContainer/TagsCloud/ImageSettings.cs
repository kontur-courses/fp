using System.Drawing;
using TagsCloudContainer.Interfaces;

namespace TagsCloudContainer.TagsCloud
{
    public class ImageSettings : IImageSettings, IDisposable
    {
        private readonly Font font;

        public ImageSettings()
        {
            font = new Font("Verdana", 20);
        }

        public Color BackgroundColor { get; init; } = Color.White;
        public Color FontColor { get; init; } = Color.Black;
        public int Width { get; set; } = 1600;
        public int Height { get; set; } = 1200;

        public Font GetFont()
        {
            return font;
        }

        public void UpdateImageSettings(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                font.Dispose();
            }
        }

        ~ImageSettings()
        {
            Dispose(false);
        }


    }
}
