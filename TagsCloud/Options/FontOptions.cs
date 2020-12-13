using System;
using System.Drawing;
using System.Text.RegularExpressions;

namespace TagsCloud.Options
{
    public class FontOptions : IFontOptions
    {
        private static readonly Regex ColorParser =
            new Regex(@"argb\((?<alpha>\d{1,3}),(?<red>\d{1,3}),(?<green>\d{1,3}),(?<blue>\d{1,3})\)");

        private string FontColorString { get; }
        public string FontFamily { get; }
        public Color FontColor { get; }

        public FontOptions(string fontFamily, string fontColor)
        {
            FontFamily = fontFamily;
            FontColorString = fontColor;
            FontColor = StringToArgbColor(fontColor);
        }

        private static Color StringToArgbColor(string s)
        {
            var random = new Random();
            if (s == "random")
                return Color.FromArgb(random.Next(255), random.Next(255), random.Next(255));
            var match = ColorParser.Match(s).Groups;
            return Color.FromArgb(int.Parse(match["alpha"].Value), int.Parse(match["red"].Value),
                int.Parse(match["green"].Value), int.Parse(match["blue"].Value));
        }
    }
}