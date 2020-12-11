using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TagsCloud.Layouter;

namespace TagsCloud.Drawer
{
    public class LayoutDrawer : ILayoutDrawer
    {
        private static readonly Regex ColorParser =
            new Regex(@"argb\((?<alpha>\d{1,3}),(?<red>\d{1,3}),(?<green>\d{1,3}),(?<blue>\d{1,3})\)");

        private readonly Random random;
        private IEnumerable<Tag> layoutTags;

        public LayoutDrawer()
        {
            random = new Random();
            layoutTags = new List<Tag>();
        }

        public void AddTags(IEnumerable<Tag> tags)
        {
            layoutTags = tags;
        }

        public void Draw(Graphics graphics)
        {
            foreach (var tag in layoutTags)
            {
                var brush = new SolidBrush(StringToArgbColor(tag.FontColor));
                using var font = new Font(tag.FontFamily, tag.FontSize);
                graphics.DrawString(tag.Text, font, brush, tag.Rectangle.Location);
            }
        }

        private Color StringToArgbColor(string s)
        {
            if (s == "random")
                return Color.FromArgb(random.Next(255), random.Next(255), random.Next(255));
            var match = ColorParser.Match(s).Groups;
            return Color.FromArgb(int.Parse(match["alpha"].Value), int.Parse(match["red"].Value),
                int.Parse(match["green"].Value), int.Parse(match["blue"].Value));
        }

        private static bool IsCorrectColor(GroupCollection colors)
        {
            return colors["alpha"].Value != "" && colors["red"].Value != "" && colors["green"].Value != "" &&
                   colors["blue"].Value != "";
        }
    }
}