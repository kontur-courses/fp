using System;
using System.ComponentModel;
using System.Drawing;
using TagCloud.CloudLayouter;
using TagCloud.Factories;

namespace TagCloud.CloudVisualizerSpace.CloudViewConfigurationSpace
{
    public class CloudViewConfiguration
    {
        [Browsable(false)]
        public IFigurePathFactory FigurePath { get; }

        [Browsable(false)]
        public Func<ICloudLayouter> CloudLayouter { get; }
        public int WordsCount { get; set; }
        public double ScaleCoefficient { get; set; }
        public Size ImageSize { get; set; }
        public Point CloudCenter { get; set; }

        [Browsable(false)]
        public Result<FontFamily> FontFamily { get; set; }
        public Color BackgroundColor { get; set; }
        public Color TextColor { get; set; }
        public bool NeedSnuggle { get; set; }

        public Brush GetBrush()
        {
            return new SolidBrush(TextColor);
        }


        public CloudViewConfiguration(IFigurePathFactory figureFactory, Func<ICloudLayouter> createCloudLayouter, IColorWordPicker colorWordPicker)
        {
            FigurePath = figureFactory;
            CloudLayouter = createCloudLayouter;
            InitializeDefaultValues();
        }

        public bool SetFontFamily(string fontFamily)
        {
            FontFamily = Result.Of(() => new FontFamily(fontFamily), "Font is not found");
            return FontFamily.IsSuccess;
        }

        private void InitializeDefaultValues()
        {
            WordsCount = 10;
            ScaleCoefficient = 100;
            ImageSize = new Size(600, 300);
            CloudCenter = new Point(300, 150);
            try
            {
                FontFamily = Result.Ok(new FontFamily("Algerian"));
            }
            catch (Exception)
            {
                FontFamily = Result.Ok(System.Drawing.FontFamily.GenericSansSerif);
            }
            BackgroundColor = Color.Green;
            TextColor = Color.White;
        }
    }
}
