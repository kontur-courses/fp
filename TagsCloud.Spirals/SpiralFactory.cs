using System.Drawing;
using TagsCloud.Common;

namespace TagsCloud.Spirals
{
    public class SpiralFactory : ISpiralFactory
    {
        private readonly SpiralSettings settings;

        public SpiralFactory(SpiralSettings settings)
        {
            this.settings = settings;
        }

        public ISpiral Create(Point center) => new ArchimedeanSpiral(center, settings);
    }
}