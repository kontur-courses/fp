using System;
using System.Drawing;
using System.IO;
using Newtonsoft.Json;
using ResultOf;

namespace TagsCloudContainer.TagCloudVisualization
{
    public class DefaultVisualizationSettings : VisualizationSettings
    {
        public string FontName => Font.Name.ToLower();
        public int FontSize => (int) Font.Size;
        public string BackgroundColorName => ((SolidBrush) BackgroundBrush).Color.Name.ToLower();
        public string TextColorName => ((SolidBrush) TextBrush).Color.Name.ToLower();
        public string SizeOfImage => $"{ImageSize.Width}x{ImageSize.Height}";
        public string DebugMarkingColorName => DebugMarkingColor.Name.ToLower();
        public string DebugItemBoundsColorName => DebugItemBoundsColor.Name.ToLower();

        private readonly string format;
        public string Format => format.ToLower();

        public DefaultVisualizationSettings(Font font, Brush backgroundBrush, Brush textBrush, bool isDebugMode,
            Color debugMarkingColor, Color debugItemBoundsColor, string format, Size size)
            : base(font, backgroundBrush, textBrush, isDebugMode, debugMarkingColor, debugItemBoundsColor, size)
        {
            this.format = format;
        }

        public static Result<DefaultVisualizationSettings> Create()
        {
            return Result.Of(() => Path.Combine(Directory.GetParent(Environment.CurrentDirectory)
                    .Parent?.Parent?.FullName, "TagCloudVisualization", "defaultSettings.json"))
                .Then(File.ReadAllText)
                .RefineError("Error accessing default settings file")
                .Then(jsonDefaultSettings => JsonConvert
                    .DeserializeObject<JsonDefaultVisualizationSettings>(jsonDefaultSettings)
                    .ToDefaultVisualizationSettings().GetValueOrThrow())
                .RefineError("Error creating default settings");
        }
    }
}