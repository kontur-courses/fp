using System.Drawing;
using Newtonsoft.Json;
using ResultOf;

namespace TagsCloudContainer.TagCloudVisualization
{
    public class VisualizationSettings
    {
        public Font Font { get; set; }
        public Brush BackgroundBrush { get; set; }
        public Brush TextBrush { get; set; }
        public Size ImageMaxSize { set; get; }
        public bool IsDebugMode { get; set; }
        public Color DebugMarkingColor { get; set; }
        public Color DebugItemBoundsColor { get; set; }

        public static VisualizationSettings Empty => new VisualizationSettings(null, null, 
            null, false, default(Color), default(Color),
            default(Size));

        protected VisualizationSettings(Font font, Brush backgroundBrush, Brush textBrush, bool isDebugMode,
            Color debugMarkingColor, Color debugItemBoundsColor, Size imageMaxSize)
        {
            Font = font;
            BackgroundBrush = backgroundBrush;
            TextBrush = textBrush;
            IsDebugMode = isDebugMode;
            DebugMarkingColor = debugMarkingColor;
            DebugItemBoundsColor = debugItemBoundsColor;
            ImageMaxSize = imageMaxSize;
        }

        public static Result<VisualizationSettings> CreateDefaultSettings()
        {
            return AppSettings.CreateDefault()
                .Then(settings => (VisualizationSettings) settings);
        }
    }
}