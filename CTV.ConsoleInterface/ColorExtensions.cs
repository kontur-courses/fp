using System.Drawing;

namespace CTV.ConsoleInterface
{
    public static class ColorExtensions
    {
        public static Color WithRed(this Color color, int red)
        {
            return Color.FromArgb(red, color.G, color.B);
        }

        public static Color WithGreen(this Color color, int green)
        {
            return Color.FromArgb(color.R, green, color.B);
        }

        public static Color WithBlue(this Color color, int blue)
        {
            return Color.FromArgb(color.R, color.G, blue);
        }
    }
}