using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CommandLine;

namespace TagCloudConsoleUI
{
    [Verb("draw", HelpText = "Draw tag cloud")]
    public class DrawOptions
    {
        private List<Color> innerColors = new (){ Color.Magenta };
        
        [Option('b', "background-color", Required = false, HelpText = "Set background color")]
        public Color BackgroundColor { get; set; } = Color.Black;

        [Option('c', "colors", Required = false, HelpText = "Set colors for words")]
        public IEnumerable<Color> InnerColors
        {
            get => innerColors;
            set
            {
                var valueList = value.ToList();
                if (valueList.Count > 0)
                    innerColors = valueList;
            }
        }

        [Option('h', "image-height", Required = false, HelpText = "Set result image height")]
        public int Height { get; set; } = 1500;
        
        [Option('w', "image-width", Required = false, HelpText = "Set result image width")]
        public int Width { get; set; } = 1500;
    }
}