using System.Collections.Generic;
using System.Drawing;

namespace TagCloud.Visualization
{
    public class ViewSettings
    {
        public readonly HashSet<Brush> Colors = new HashSet<Brush>
        {
            Brushes.Aqua, Brushes.Lime, Brushes.Blue, Brushes.Brown, Brushes.Chartreuse,
            Brushes.Chocolate, Brushes.Coral, Brushes.Crimson, Brushes.MediumSlateBlue,
            Brushes.Gold, Brushes.Green, Brushes.Fuchsia, Brushes.BlueViolet
        };

        public string FontName { get; set; } = "courier new";

        public Color BackgroundColor { get; set; } = Color.DarkBlue;

        public bool EnableWordRectangles { get; set; } = false;

        public int WordsCount { get; set; } = 50;
    }
}