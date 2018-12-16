using CommandLine;
using System.Drawing;
using TagCloud.Core.Settings.Interfaces;

namespace TagCloud.CUI
{
    public class Options : ILayoutingSettings, IPaintingSettings, ITagCloudSettings, IVisualizingSettings, ITextParsingSettings
    {
        #region LayotingSettings
        [Option("spiralstep", Default = 1e-2)]
        public double SpiralStepMultiplier { get; set; }
        #endregion


        #region PaintingSettings
        [Option("backgroundcolor", Default = "white")]
        public string BackgroundColorName { get; set; }

        [Option("tagbrush", Default = "black")]
        public string TagBrushName { get; set; }

        public Color BackgroundColor => Color.FromName(BackgroundColorName);
        public Brush TagBrush => new SolidBrush(Color.FromName(TagBrushName));
        #endregion


        #region TagCloudSettings
        [Option('p', "pathtowords", Required = true)]
        public string PathToWords { get; set; }

        [Option('b', "pathtoboringwords")] public string PathToBoringWords { get; set; }

        [Option('i', "imagepath", Default = "result.png")]
        public string PathForResultImage { get; set; }
        #endregion


        #region TextParsingOptions
        [Option('c', "maxtagscount")]
        public int? MaxUniqueWordsCount { get; set; }
        #endregion


        #region VisualizingOptions
        [Option('w', "width", Default = 800)]
        public int Width { get; set; } = 800;

        [Option('h', "height", Default = 600)]
        public int Height { get; set; } = 600;

        [Option('f', "font", Default = "arial")]
        public string FontName { get; set; } = "arial";

        [Option("minfontsize", Default = 15)]
        public float MinFontSize { get; set; } = 15;

        [Option("maxfontsize", Default = 35)]
        public float MaxFontSize { get; set; } = 35;

        public PointF CenterPoint => new PointF((float)Width / 2, (float)Height / 2);
        public Font DefaultFont => new Font(FontName, (MaxFontSize + MinFontSize) / 2);
        #endregion
    }
}