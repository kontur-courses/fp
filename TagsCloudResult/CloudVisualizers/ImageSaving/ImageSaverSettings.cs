using System.Drawing.Imaging;

namespace TagsCloudResult.CloudVisualizers.ImageSaving
{
    public class ImageSaverSettings
    {
        public ImageFormat Format { get; set; } = ImageFormat.Jpeg;
        public string Path { get; set; }
    }
}