using System.Drawing;
using TagsCloudContainer.Settings.Interfaces;

namespace TagsCloudContainer.Settings
{
    public class RenderingSettings : IRenderingSettings
    {
        public Size? DesiredImageSize { get; set; }
        public float Scale { get; set; }

        public Brush Background
        {
            get => background;
            set
            {
                background.Dispose();
                background = value;
            }
        }

        private Brush background = new SolidBrush(Color.Transparent);
        
        // public RenderingSettings(RenderSettings settings)
        // {
        //     DesiredImageSize = ValidateSize(settings.ImageSize);
        //     Scale = Validate.Positive("Image scale", settings.ImageScale);
        //     Background = new SolidBrush(settings.BackgroundColor);
        // }
        //
        // private static Size? ValidateSize(Size? size)
        // {
        //     if (!size.HasValue)
        //         return size;
        //
        //     Validate.Positive("Image height", size.Value.Height);
        //     Validate.Positive("Image width", size.Value.Width);
        //
        //     return size;
        // }
        //
        // public void Dispose()
        // {
        //     Background.Dispose();
        // }
    }
}