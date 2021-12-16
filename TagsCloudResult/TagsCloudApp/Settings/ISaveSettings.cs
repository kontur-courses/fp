using System.Drawing.Imaging;

namespace TagsCloudApp.Settings
{
    public interface ISaveSettings
    {
        string OutputFile { get; set; }
        ImageFormat ImageFormat { get; set; }
    }
}