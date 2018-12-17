using System.Drawing;

namespace TagsCloudResult.Settings
{
    public interface ICloudSettings
    {
        int WordsToDisplay { get; }
        Point CenterPoint { get; }
        Size Size { get; }
    }
}
