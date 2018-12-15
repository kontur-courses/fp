using System;
using System.Drawing;
using System.Drawing.Imaging;
using TagsCloudVisualization.PointGenerators;

namespace TagsCloudVisualization
{
    public class CloudParameters
    {
        public CloudParameters(Size imageSize, Func<float, Color> colorFunc,
            string fontName, IPointGenerator pointGenerator, ImageFormat outFormat)
        {
            ImageSize = imageSize;
            ColorFunc = colorFunc;
            FontName = fontName;
            PointGenerator = pointGenerator;
            OutFormat = outFormat;
        }

        public Size ImageSize { get; set; }
        public Func<float, Color> ColorFunc { get; set; }
        public string FontName { get; set; }
        public IPointGenerator PointGenerator { get; set; }
        public ImageFormat OutFormat { get; set; }
    }
}