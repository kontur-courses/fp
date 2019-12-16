using System.Drawing;

namespace TagsCloudGenerator.DTO
{
    public class WordDrawingDTO
    {
        public string Word { get; set; }
        public string FontName { get; set; }
        public float MaxFontSymbolWidth { get; set; }
        public RectangleF WordRectangle { get; set; }
    }
}