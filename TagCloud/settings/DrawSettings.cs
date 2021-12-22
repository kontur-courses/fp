using System.Collections.Generic;
using System.Drawing;

namespace TagCloud.settings
{
    public class DrawSettings : IDrawSettings
    {
        private readonly List<Color> innerColors;
        private readonly Color backgroundColor;
        private readonly Size size;

        public DrawSettings(List<Color> innerColors, Color backgroundColor, Size size)
        {
            this.innerColors = innerColors;
            this.backgroundColor = backgroundColor;
            this.size = size;
        }


        public List<Color> GetInnerColors() => innerColors;

        public Color GetBackgroundColor() => backgroundColor;

        public Size GetSize() => size;
    }
}