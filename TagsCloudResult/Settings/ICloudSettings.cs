using System.Drawing;

namespace TagsCloudResult.Settings
{
    public interface ICloudSettings
    {
        int WordsToDisplay { get; set; }
        Point CenterPoint { get; }
        Size Size { get; set; }
    }
}
