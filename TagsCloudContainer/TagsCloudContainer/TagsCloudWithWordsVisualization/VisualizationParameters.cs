using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudContainer.TagsCloudWithWordsVisualization
{
    public class VisualizationParameters
    {
        public readonly List<Color> TagColors;
        public readonly Color BackgroundColor;
        public readonly SizeRange TagSizeRange;
        public readonly FontFamily FontFamily;
        public readonly float MinFontSize;
        public readonly List<Brush> TextBrushes;

        public VisualizationParameters(List<Color> tagColors, Color backgroundColor, SizeRange tagSizeRange, FontFamily fontFamily, float minFontSize, List<Brush> textBrushes)
        {
            TagColors = tagColors;
            BackgroundColor = backgroundColor;
            TagSizeRange = tagSizeRange;
            FontFamily = fontFamily;
            MinFontSize = minFontSize;
            TextBrushes = textBrushes;
        }
    }
}