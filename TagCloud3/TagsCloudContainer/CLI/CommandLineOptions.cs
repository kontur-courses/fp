using CommandLine;
using ResultOf;
using System.Drawing;
using System.Drawing.Text;
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
            SettingsStorage.AppSettings = new AppSettings();
            SettingsStorage.AppSettings.DrawingSettings = new();

            SettingsStorage.AppSettings.TextFile = options.Filename;
            SettingsStorage.AppSettings.OutImagePath = options.Output;
            SettingsStorage.AppSettings.FilterFile = options.Filter;

            SettingsStorage.AppSettings.DrawingSettings.FontFamily = GetFontFamily(options.FontFamily).GetValueOrDefault(new FontFamily("Arial"));
            SettingsStorage.AppSettings.DrawingSettings.FontSize = GetFontSize(options.FontSize).GetValueOrDefault(12);
            SettingsStorage.AppSettings.DrawingSettings.Size = GetSize(options.Size).GetValueOrDefault(new Size(800, 600));
            SettingsStorage.AppSettings.DrawingSettings.Colors = GetColors(options.Colors);
            SettingsStorage.AppSettings.DrawingSettings.PointsProvider = GetPointsProvider(options.Layout).GetValueOrDefault(new NormalPointsProvider(new Point(0, 0)));
            SettingsStorage.AppSettings.DrawingSettings.BgColor = GetBGColor(options.BgColor);

            return SettingsStorage.AppSettings;
        }

        private static Result<int> GetFontSize(string fontSize)
        {
            int size;
            if (int.TryParse(fontSize, out size) && size > 0)
            {
                return Result<int>.Ok(size);
            }

            return Result<int>.Fail($"Font size '{fontSize}' is not valid. Should be positive Integer.");
        }

        private static Result<Size> GetSize(string sizeString)
        {
            if (string.IsNullOrEmpty(sizeString))
            {
                return Result<Size>.Fail($"Size is empty. Should be two positive Integer, sepparated by comma.");
            }

            var s = sizeString.Split(',', ' ', StringSplitOptions.RemoveEmptyEntries);

            int width;
            int height;

            if (s.Count() == 2 && int.TryParse(s[0], out width) && int.TryParse(s[1], out height))
            {
                if (width > 0 && height > 0)
                {
                    return Result<Size>.Ok(new Size(width, height));
                }
            }
            return Result<Size>.Fail($"Size format '{sizeString}' is not valid. Should be two positive Integer, sepparated by comma.");
        }

        private static Result<IPointsProvider> GetPointsProvider(string layout)
        {
            if (string.IsNullOrEmpty(layout))
            {
                return Result<IPointsProvider>.Fail("Point provider name is null or empty");
            }

            IPointsProvider pointProvider;

            switch (layout.ToLowerInvariant())
            {
                case "random":
                    pointProvider = new RandomPointsProvider(new Point(0, 0));
                    break;

                case "normal":
                    pointProvider = new NormalPointsProvider(new Point(0, 0));
                    break;

                case "spiral":
                    pointProvider = new SpiralPointsProvider(new Point(0, 0));
                    break;

                default:
                    return Result<IPointsProvider>.Fail($"Can't find Point provider with name {layout}");
            }

            return Result<IPointsProvider>.Ok(pointProvider);
        }

        private static Result<FontFamily> GetFontFamily(string fontName)
        {

            if (string.IsNullOrEmpty(fontName))
            {
                return Result<FontFamily>.Fail("Font name is null or empty");
            }

            var fontsCollection = new InstalledFontCollection();
            if (!fontsCollection.Families.Any(x => x.Name.ToLowerInvariant() == fontName.ToLowerInvariant()))
            {
                return Result<FontFamily>.Fail($"Font {fontName} not found. Check the font name and make sure that the font is installed.");
            }

            return Result<FontFamily>.Ok(new FontFamily(fontName));
        }

        private static IList<Color> GetColors(string colors)
        {
            if (string.IsNullOrEmpty(colors))
            {
                return new List<Color>() { Color.White };
            }

            var parsedColors = colors.Split(',').Select(x => Color.FromName(x)).ToList();

            if (parsedColors.Count > 0)
            {
                return parsedColors.ToList();
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