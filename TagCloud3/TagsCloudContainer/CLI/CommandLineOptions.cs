using CommandLine;
using ResultOf;
using System.Drawing;
using System.Drawing.Text;
using System.Runtime.CompilerServices;
using TagsCloudContainer.SettingsClasses;
using TagsCloudContainer.TagCloudBuilder;
using TagsCloudVisualization;

namespace TagsCloudContainer.CLI
{
    public class CommandLineOptions
    {
        [Option('f', "filename", Required = true, HelpText = "Input file name.")]
        public string Filename { get; set; }

        [Option('o', "output", Required = true, HelpText = "Output file name.")]
        public string Output { get; set; }

        [Option("font", Required = false, HelpText = "Set the font family name.")]
        public string FontFamily { get; set; }

        [Option("fontsize", Required = false, HelpText = "Set the font size. Must be a positive integer.")]
        public string FontSize { get; set; }

        [Option("colors", Required = false, HelpText = "List of color names. Separated by commas.")]
        public string Colors { get; set; }

        [Option("size", Required = false, HelpText = "Set the image size. Must be two positive integer.")]
        public string Size { get; set; }

        [Option("exclude", Required = false, HelpText = "File with words to exclude.")]
        public string Filter { get; set; }

        [Option("layout", Required = false, HelpText = "Set cloud layouter - Spiral, Random or Normal.")]
        public string Layout { get; set; }

        [Option("background", Required = false, HelpText = "Set background color.")]
        public string BgColor { get; set; }

        public static AppSettings ParseArgs(CommandLineOptions options)
        {
            var appSettings = new AppSettings();
            appSettings.DrawingSettings = new();

            appSettings.TextFile = options.Filename;
            appSettings.OutImagePath = options.Output;
            appSettings.FilterFile = options.Filter;

            appSettings.DrawingSettings.FontFamily = GetFontFamily(options.FontFamily).GetValueOrDefault(new FontFamily("Arial"));
            appSettings.DrawingSettings.FontSize = GetFontSize(options.FontSize).GetValueOrDefault(12);
            appSettings.DrawingSettings.Size = GetSize(options.Size).GetValueOrDefault(new Size(800,600));
            appSettings.DrawingSettings.Colors = GetColors(options.Colors);
            appSettings.DrawingSettings.PointsProvider = GetPointsProvider(options.Layout).GetValueOrDefault(new NormalPointsProvider());
            appSettings.DrawingSettings.BgColor = GetBGColor(options.BgColor);

            return appSettings;
        }

        private static Result<int> GetFontSize(string fontSize)
        {
            int size;
            if (int.TryParse(fontSize, out size) && size > 0)
            {
                return Result.Ok<int>(size);
            }

            return Result.Fail<int>($"Font size '{fontSize}' is not valid. Should be positive Integer.");
        }

        private static Result<Size> GetSize(string sizeString)
        {
            if(string.IsNullOrEmpty(sizeString))
            {
                return Result.Fail<Size>($"Size is empty. Should be two positive Integer, sepparated by comma.");
            }

            var s = sizeString.Split(',', ' ', StringSplitOptions.RemoveEmptyEntries);

            int width;
            int height;

            if(s.Count() == 2 && int.TryParse(s[0], out width) && int.TryParse(s[1], out height))
            {
                if(width > 0 && height >0)
                {
                    return Result.Ok<Size>(new Size(width,height));
                }
            }
            return Result.Fail<Size>( $"Size format '{sizeString}' is not valid. Should be two positive Integer, sepparated by comma." );
        }

        private static Result<IPointsProvider> GetPointsProvider(string layout)
        {
            if (string.IsNullOrEmpty(layout))
            {
                return Result.Fail<IPointsProvider>("Point provider name is null or empty");
            }

            IPointsProvider pointProvider;

            switch (layout.ToLowerInvariant())
            {
                case "random":
                    pointProvider = new RandomPointsProvider();
                    break;

                case "normal":
                    pointProvider = new NormalPointsProvider();
                    break;

                case "spiral":
                    pointProvider = new SpiralPointsProvider();
                    break;

                default:
                    return Result.Fail<IPointsProvider>($"Can't find Point provider with name {layout}");
            }

            return Result.Ok<IPointsProvider>(pointProvider);
        }

        private static Result<FontFamily> GetFontFamily(string fontName)
        {

            if (string.IsNullOrEmpty(fontName))
            {
                return Result.Fail<FontFamily>("Font name is null or empty");
            }

            var fontsCollection = new InstalledFontCollection();
            if (!fontsCollection.Families.Any(x => x.Name.ToLowerInvariant() == fontName.ToLowerInvariant()))
            {
                return Result.Fail<FontFamily>($"Font {fontName} not found. Check the font name and make sure that the font is installed.");
            }

            return Result.Ok<FontFamily>(new FontFamily(fontName));
        }

        private static IList<Color> GetColors(string colors)
        {
            if (string.IsNullOrEmpty(colors))
            {
                return new List<Color>() { Color.White };
            }

            var c = colors.Split(',').Select(x => Color.FromName(x)).ToList();

            if (c.Count > 0)
            {
                return c.ToList();
            }

            return new List<Color>() { Color.White };
        }

        private static Color GetBGColor(string colorName)
        {
            if (string.IsNullOrEmpty(colorName))
            {
                return Color.Black;
            }

            return Color.FromName(colorName);
        }
    }
}