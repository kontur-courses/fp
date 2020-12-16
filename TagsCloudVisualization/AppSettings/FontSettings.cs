using System.Drawing;

namespace TagsCloudVisualization.AppSettings
{
    public class FontSettings
    {
        public Font Font { get; set; } = new Font("Times New Roman", 14);
        public uint MinSize { get; set; } = 7;
        public uint MaxSize { get; set; } = 55;
    }
}