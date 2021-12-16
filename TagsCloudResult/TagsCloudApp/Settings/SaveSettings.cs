using System.Drawing.Imaging;
using TagsCloudApp.Parsers;
using TagsCloudApp.RenderCommand;

namespace TagsCloudApp.Settings
{
    class SaveSettings : ISaveSettings
    {
        public string OutputFile { get; set; } = "output.txt";
        public ImageFormat ImageFormat { get; set; } = ImageFormat.Png;
    }
}