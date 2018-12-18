using System.Collections.Generic;
using System.Linq;
using TagsCloudContainer.Painter;

namespace TagsCloudContainer.Settings
{
    public class ImageSettings
    {
        private readonly Dictionary<string, ICloudColorPainter> painters;

        public ImageSettings(ICloudColorPainter[] painters)
        {
            this.painters = new Dictionary<string, ICloudColorPainter>();
            foreach (var painter in painters)
                this.painters[painter.GetType().Name.Split('.').Last()] = painter;
        }

        public int Width { get; set; } = 800;
        public int Height { get; set; } = 600;
        public PainterType CloudPainter { get; set; } = PainterType.GradientPainter;

        public ICloudColorPainter GetCloudPainterClass()
        {
            return painters[CloudPainter.ToString()];
        }
    }
}