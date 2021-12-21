using System.Drawing;
using CommandLine;

namespace TagCloudApp.Configurations
{
    public class Options
    {
#pragma warning disable 8618

        [Option('i', "input", Required = true, HelpText = "File with words to draw")]
        public string Filename { get; set; }

        [Option('w', "width", Default = 1920, Required = false, HelpText = "Image width")]
        public int Width { get; set; }

        [Option('h', "height", Default = 1080, Required = false, HelpText = "Image height")]
        public int Height { get; set; }

        [Option('o', "output", Required = true, HelpText = "Output image")]
        public string Output { get; set; }

        [Option('b', "background", Required = false, HelpText = "Background color")]
        public Color BackgroundColor { get; set; }

        [Option('c', "color", Required = false, HelpText = "Text color, default is random generating")]
        public Color Color { get; set; } = Color.Empty;

        [Option('f', "font-family", Required = false, HelpText = "Words font-family")]
        public FontFamily? FontFamily { get; set; }

        [Option('g', "form of cloud (spiral, circle)", Required = false, HelpText = "You can configure form of cloud")]
        public string? CloudForm { get; set; }
#pragma warning restore 8618
    }
}