using System.Drawing.Imaging;

namespace TagsCloudApp.Settings
{
    public class SaveSettings : ISaveSettings
    {
        public string OutputFile { get; set; } = "output.txt";
        public ImageFormat ImageFormat { get; set; } = ImageFormat.Png;
    }
}