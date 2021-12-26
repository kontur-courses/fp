using System.Drawing;

namespace TagsCloudContainer
{
    internal static class AppSettings
    {
        public static string TextFilename { get; set; } = "..\\..\\..\\Tags\\startTags.txt";
        public static string ImageFilename { get; set; } = "..\\..\\..\\images\\image1.jpg";

        public static Size ImageSize { get; set; } = new Size(800, 800);
        public static FontFamily FontFamily { get; set; } = FontFamily.GenericSerif;
        public static Color BackgroundColor { get; set; } = Color.White;
        public static float MinMargin { get; set; } = 10;
        public static bool FillTags { get; set; }

        public static int MinTagHeight { get; set; } = 12;
        public static float MaxTagSizeScale { get; set; } = 10;
    }
}