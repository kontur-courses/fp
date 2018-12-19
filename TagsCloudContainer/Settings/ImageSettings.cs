using System.Drawing;
using TagsCloudContainer.Themes;

namespace TagsCloudContainer.Settings
{
    public class ImageSettings
    {
        public ImageSettings(Option option)
        {
            OutputFile = option.OutputFile;
            Theme = GetThemeByName(option.Theme);
            Height = option.Height;
            Width = option.Width;
            Center = new Point(Height / 2, Width / 2);
        }

        public int Height { get; }
        public int Width { get; }
        public Point Center { get; }
        public string OutputFile { get; }
        public ITheme Theme { get; }


        private ITheme GetThemeByName(string theme)
        {
            return Themes.Themes.ThemesDictionary[theme];
        }
    }
}