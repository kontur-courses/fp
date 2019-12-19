using System;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using CommandLine;
using TagsCloudVisualization.ErrorHandling;
using TagsCloudVisualization.Visualization;

namespace TagsCloudVisualization.ApplicationOptions
{
    public class ApplicationOptions
    {
        [Option('o', "font", HelpText = "Font family name", Default = "Arial")]
        public string FontFamily
        {
            get => fontFamily;
            set => fontFamily = IsFontInstalled(value) ? value : throw new ArgumentException("Font not installed");
        }

        [Option('s', "fontSize", HelpText = "Font size", Default = 10)]
        public int MinFontSize
        {
            get => minFontSize;
            set => minFontSize = value > 0 ? value : throw new ArgumentException("Font size must be positive");
        }

        [Option('w', "imageWidth", HelpText = "Image width", Default = 1000)]
        public int ImageWidth
        {
            get => imageWidth;
            set => imageWidth = value > 0 ? value : throw new ArgumentException("Width must be positive");
        }

        [Option('n', "imageHeight", HelpText = "Image height", Default = 1000)]
        public int ImageHeight
        {
            get => imageHeight;
            set => imageHeight = value > 0 ? value : throw new ArgumentException("Height must be positive");
        }

        [Option('b', "backgroundColor", HelpText = "Background color", Default = "Black")]
        public string BackGroundColorName
        {
            get => backGroundColorName;
            set => backGroundColorName = Color.FromName(value).IsKnownColor
                ? value
                : throw new ArgumentException("Unknown background color");
        }

        [Option('d', "textColor", HelpText = "Text color", Default = "Pink")]
        public string TextColorName
        {
            get => textColorName;
            set => textColorName = Color.FromName(value).IsKnownColor
                ? value
                : throw new ArgumentException("Unknown text color");
        }

        [Option('t', "textPath", HelpText = "Text path", Required = true)]
        public string TextPath { get; set; }

        [Option('i', "imagePath", HelpText = "Image path", Required = true)]
        public string ImagePath { get; set; }

        [Option('y', "boringWords", HelpText = "Boring words")]
        public string BoringWords { get; set; }
        
        private int minFontSize;
        private string fontFamily;
        private int imageWidth;
        private int imageHeight;
        private string backGroundColorName;
        private string textColorName;

        public Result<VisualisingOptions> GetVisualizingOptions()
        {
            return Result.Of(() => new VisualisingOptions(new Font(FontFamily, MinFontSize),
                new Size(ImageWidth, ImageHeight), Color.FromName(BackGroundColorName), Color.FromName(TextColorName)));
        }

        private bool IsFontInstalled(string fontName)
        {
            using (var fonts = new InstalledFontCollection())
            {
                return fonts.Families.Any(f =>
                    string.Equals(f.Name, fontName, StringComparison.CurrentCultureIgnoreCase));
            }
        }
    }
}