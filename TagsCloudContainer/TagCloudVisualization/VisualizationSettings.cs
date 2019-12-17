using System.Drawing;
using ResultOf;

namespace TagsCloudContainer.TagCloudVisualization
{
    public class VisualizationSettings
    {
        public Font Font { get; set; }
        public Brush BackgroundBrush { get; set; }
        public Brush TextBrush { get; set; }
        public Size ImageSize { set; get; }
        public bool IsDebugMode { get; set; }
        public Color DebugMarkingColor { get; set; }
        public Color DebugItemBoundsColor { get; set; }

        public static VisualizationSettings Empty => new VisualizationSettings(null, null, 
            null, false, default(Color), default(Color),
            default(Size));

        protected VisualizationSettings(Font font, Brush backgroundBrush, Brush textBrush, bool isDebugMode,
            Color debugMarkingColor, Color debugItemBoundsColor, Size imageSize)
        {
            Font = font;
            BackgroundBrush = backgroundBrush;
            TextBrush = textBrush;
            IsDebugMode = isDebugMode;
            DebugMarkingColor = debugMarkingColor;
            DebugItemBoundsColor = debugItemBoundsColor;
            ImageSize = imageSize;
        }

        public static Result<VisualizationSettings> CreateDefaultSettings()
        {
            return Result.Of<VisualizationSettings>(DefaultVisualizationSettings.Create().GetValueOrThrow);
        }
    }
}