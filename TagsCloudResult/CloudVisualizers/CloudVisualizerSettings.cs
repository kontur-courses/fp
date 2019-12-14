using System.Drawing;

namespace TagsCloudResult.CloudVisualizers
{
    public class CloudVisualizerSettings
    {
        public Palette Palette { get; set; }
        public IBitmapMaker BitmapMaker { get; set; }
        public int Width { get; set; } = 1280;
        public int Height { get; set; } = 720;
        
        public Font Font { get; set; } = new Font("Arial", 16);
    }
}