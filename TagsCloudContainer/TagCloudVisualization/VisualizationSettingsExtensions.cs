using System.Drawing;
using FluentAssertions;

namespace TagsCloudContainer.TagCloudVisualization
{
    public static class VisualizationSettingsExtensions
    {
        public static VisualizationSettings WithFont(this VisualizationSettings settings, Font font)
        {
            settings.Font = font;
            return settings;
        }
        
        public static VisualizationSettings WithBackgroundBrush(this VisualizationSettings settings, Brush backgroundBrush)
        {
            settings.BackgroundBrush = backgroundBrush;
            return settings;
        }
        
        public static VisualizationSettings WithTextBrush(this VisualizationSettings settings, Brush textBrush)
        {
            settings.TextBrush = textBrush;
            return settings;
        }
        
        public static VisualizationSettings WithDebugMode(this VisualizationSettings settings, bool isDebugMode)
        {
            settings.IsDebugMode = isDebugMode;
            return settings;
        }
        
        public static VisualizationSettings WithDebugMarkingColor(this VisualizationSettings settings, Color debugMarkingColor)
        {
            settings.DebugMarkingColor = debugMarkingColor;
            return settings;
        }
        
        public static VisualizationSettings WithDebugItemRectangleColor(this VisualizationSettings settings, Color debugItemRectangleColor)
        {
            settings.DebugItemBoundsColor = debugItemRectangleColor;
            return settings;
        }
        
        public static VisualizationSettings WithSize(this VisualizationSettings settings, Size size)
        {
            settings.ImageMaxSize = size;
            return settings;
        }
    }
}