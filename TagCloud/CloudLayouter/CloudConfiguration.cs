using System;
using System.ComponentModel;
using System.Drawing;
using TagCloud.Factories;

namespace TagCloud.CloudLayouter
{
    public class CloudConfiguration
    {
        [Browsable(false)]
        public IFigurePathFactory FigurePath { get; }

        [Browsable(false)]
        public Func<ICloudLayouter> CloudLayouter { get; }
        public int WordsCount { get; set; }
        public Point CloudCenter { get; set; }
        public bool NeedSnuggle { get; set; }

        public CloudConfiguration(IFigurePathFactory figurePath, Func<ICloudLayouter> createCloudLayouter)
        {
            CloudLayouter = createCloudLayouter;
            FigurePath = figurePath;

            WordsCount = 10;
            CloudCenter = new Point(300, 150);
        }
    }
}
