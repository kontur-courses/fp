using System;
using System.Drawing;
using TagCloud.Core.Settings.Interfaces;
using TagCloud.Core.Util;

namespace TagCloud.Core.Settings.DefaultImplementations
{
    public class VisualizingSettings : IVisualizingSettings
    {
        public string FontName { get; set; } = "arial";
        public int Width { get; set; } = 800;
        public int Height { get; set; } = 600;
        public float MinFontSize { get; set; } = 15;
        public float MaxFontSize { get; set; } = 35;

        public PointF CenterPoint => new PointF((float) Width / 2, (float) Height / 2);

        public Result<Font> DefaultFontResult
        {
            get
            {
                return Result
                    .Of(() => new Font(FontName, (MaxFontSize + MinFontSize) / 2))
                    .Then(font => font.Name.Equals(FontName, StringComparison.CurrentCultureIgnoreCase)
                        ? font
                        : Result.Fail<Font>($"Can't load font with name \"{FontName}\""));
            }
        }
    }
}