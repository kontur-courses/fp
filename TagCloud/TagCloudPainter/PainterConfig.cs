using System.Drawing;
using System.IO;
using TagCloud.TagCloudVisualisation.Spirals;

namespace TagCloud.TagCloudPainter
{
    public class PainterConfig
    {
        public int ImageWidth;
        public int ImageHeight;
        public string ImageName;
        
        public int MaxFontSize;
        public int MinFontSize;
        
        public string PathForSave;
        
        public FontFamily FontFamily;
        public Point CloudCenter;

        public Color BackgroundColor;
        public Color[] Pallet;
        public ISpiral LayoutAlgorithm;

        public static PainterConfig Default()
        {
            return new PainterConfig()
            {
                ImageWidth = 5000,
                ImageHeight = 5000,
                MaxFontSize = 300,
                MinFontSize = 20,
                ImageName = "TestImage",
                PathForSave = Directory.GetCurrentDirectory(),
                FontFamily = FontFamily.GenericMonospace,
                CloudCenter = new Point(2500, 2500),
                LayoutAlgorithm = new ArchimedeanSpiral(),
                BackgroundColor = Color.Black,
                Pallet = new []
                {
                    Color.LawnGreen,
                    Color.RoyalBlue,
                    Color.Red,
                    Color.Orange,
                    Color.WhiteSmoke,
                    Color.Aqua,
                    Color.GreenYellow
                }
            };
        }
    }
}