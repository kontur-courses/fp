using System;
using System.Drawing;

namespace TagsCloudVisualization.Common.Settings
{
    public class TagStyleSettings : ITagStyleSettings
    {
        private string[] fontFamilies;
        private float size;
        private float sizeScatter;

        public Color[] ForegroundColors { get; set; }

        public string[] FontFamilies
        {
            get => fontFamilies;
            set
            {
                foreach (var font in value)
                    IsFontInstalled(font);
                fontFamilies = value;
            }
        }

        public float Size
        {
            get => size;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("размер шрифта для тегов должен быть больше 0.");
                size = value;
            }
        }

        public float SizeScatter
        {
            get => sizeScatter;
            set
            {
                if (value < 0)
                    throw new ArgumentException(
                        "значение разброса в размере шрифтов тегов должен быть больше или равен 0.");
                sizeScatter = value;
            }
        }

        
        public TagStyleSettings()
        {
            ForegroundColors = new[] {Color.Chocolate};
            FontFamilies = new[] {"Arial"};
            Size = 25;
            SizeScatter = 10;
        }

        public TagStyleSettings(Color[] foregroundColors, string[] fontFamilies, float size, float sizeScatter)
        {
            ForegroundColors = foregroundColors;
            FontFamilies = fontFamilies;
            Size = size;
            SizeScatter = sizeScatter;
        }

        private static void IsFontInstalled(string fontFamily)
        {
            using var fontTester = new Font(fontFamily, 1);
            if (fontTester.Name != fontFamily)
                throw new ArgumentException($"шрифт '{fontFamily}' не найден.");
        }
    }
}