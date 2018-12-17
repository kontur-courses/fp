using System.Drawing;

namespace TagsCloudResult.Settings
{
    public class DefaultCloudSettings : ICloudSettings
    {
        public int WordsToDisplay { get; } = 100;
        public Point CenterPoint => new Point(Size.Width / 2, Size.Height / 2);
        public Size Size { get; } = new Size(1500, 1500);
    }
}