using System.Drawing;
using ResultOf;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace TagsCloudContainer.TagCloudVisualization
{
    public class JsonDefaultVisualizationSettings
    {
        public string Font { get; set; }
        public int FontSize { get; set; }
        public string BackgroundBrush { get; set; }
        public string TextBrush { get; set; }
        public bool IsDebugMode { get; set; }
        public string DebugMarkingColor { get; set; }
        public string DebugItemBoundsColor { get; set; }
        public string ImageFormat { get; set; }

        public Result<DefaultVisualizationSettings> ToDefaultVisualizationSettings()
        {
            var fontResult = ArgumentsParser.ParseFont(Font, FontSize);
            var backgroundBrushResult = ArgumentsParser.ParseBrush(BackgroundBrush);
            var textBrushResult = ArgumentsParser.ParseBrush(TextBrush);
            var debugItemRectangleColorResult = ArgumentsParser.ParseColor(DebugMarkingColor);
            var debugMarkingColorResult = ArgumentsParser.ParseColor(DebugItemBoundsColor);
            var defaultSize = new Size(int.MaxValue, height: int.MaxValue);

            return Result.Of(() => VisualizationSettings.Empty)
                .Then(settings => settings.WithFont(fontResult.GetValueOrThrow()))
                .Then(settings => settings.WithBackgroundBrush(backgroundBrushResult.GetValueOrThrow()))
                .Then(settings => settings.WithTextBrush(textBrushResult.GetValueOrThrow()))
                .Then(settings => settings.WithDebugMode(IsDebugMode))
                .Then(settings =>
                    settings.WithDebugMarkingColor(debugMarkingColorResult.GetValueOrThrow()))
                .Then(settings =>
                    settings.WithDebugItemRectangleColor(debugItemRectangleColorResult.GetValueOrThrow()))
                .Then(settings => new DefaultVisualizationSettings(settings.Font,
                    settings.BackgroundBrush, settings.TextBrush, settings.IsDebugMode, settings.DebugMarkingColor,
                    settings.DebugItemBoundsColor, ImageFormat, defaultSize));
        }
    }
}