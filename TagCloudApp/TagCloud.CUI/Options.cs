using System;
using System.Drawing;
using CommandLine;
using TagCloud.Core.Settings.Interfaces;
using TagCloud.Core.Util;

namespace TagCloud.CUI
{
    public class Options : ILayoutingSettings, IPaintingSettings, ITagCloudSettings, IVisualizingSettings,
        ITextParsingSettings
    {
        #region LayotingSettings
        [Option("spiralstep", Default = 1e-2)]
        public double SpiralStepMultiplier { get; set; }
        #endregion


        #region TextParsingSettings
        [Option('c', "maxtagscount")]
        public int? MaxUniqueWordsCount { get; set; }
        #endregion


        #region PaintingSettings
        [Option("backgroundcolor", Default = "white")]
        public string BackgroundColorName { get; set; }

        [Option("tagbrush", Default = "black")]
        public string TagBrushName { get; set; }

        public Result<Color> BackgroundColorResult
        {
            get
            {
                return Result
                    .Of(() => Color.FromName(BackgroundColorName))
                    .Then(color => color.IsKnownColor
                        ? color
                        : Result.Fail<Color>($"Can't load color with name \"{BackgroundColorName}\""));
            }
        }

        public Result<Brush> TagBrushResult
        {
            get
            {
                return Result
                    .Of(() => Color.FromName(TagBrushName))
                    .Then(color => color.IsKnownColor
                        ? new SolidBrush(color)
                        : Result.Fail<Brush>($"Can't load color with name \"{TagBrushName}\""));
            }
        }
        #endregion


        #region TagCloudSettings
        [Option('p', "pathtowords", Required = true)]
        public string PathToWords { get; set; }

        [Option('b', "pathtoboringwords")]
        public string PathToBoringWords { get; set; }

        [Option('i', "imagepath", Default = "result.png")]
        public string PathForResultImage { get; set; }
        #endregion


        #region VisualizingSettings
        [Option('w', "width", Default = 800)] public int Width { get; set; } = 800;

        [Option('h', "height", Default = 600)] public int Height { get; set; } = 600;

        [Option('f', "font", Default = "arial")]
        public string FontName { get; set; } = "arial";
        
        [Option("minfontsize", Default = 15)]
        public float MinFontSize { get; set; }
        
        [Option("maxfontsize", Default = 35)]
        public float MaxFontSize { get; set; }

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
        #endregion
    }
}