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

        public Color BackgroundColor => Color.FromName(BackgroundColorName);

        public Brush TagBrush
        {
            get
            {
                var color = Color.FromName(TagBrushName);
                return color.IsKnownColor ? new SolidBrush(color) : null;
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
        [Option('w', "width", Default = 800)]
        public int Width { get; set; }

        [Option('h', "height", Default = 600)]
        public int Height { get; set; }

        [Option('f', "font", Default = "arial")]
        public string FontName { get; set; }
        
        [Option("minfontsize", Default = 15)]
        public float MinFontSize { get; set; }
        
        [Option("maxfontsize", Default = 35)]
        public float MaxFontSize { get; set; }

        public PointF CenterPoint => new PointF((float) Width / 2, (float) Height / 2);

        public Font DefaultFont
        {
            get
            {
                var font = new Font(FontName, (MaxFontSize + MinFontSize) / 2);
                return font.Name.Equals(FontName, StringComparison.CurrentCultureIgnoreCase) ? font : null;
            }
        }
        #endregion
    }
}