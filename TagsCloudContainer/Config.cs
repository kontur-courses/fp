using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudContainer.ResultRenderer;
using TagsCloudContainer.WordFormatters;
using TagsCloudContainer.WordLayouts;
using TagsCloudContainer.WordsPreprocessors;

namespace TagsCloudContainer
{
    public struct Config : ICustomWordsRemoverConfig, IResultRendererConfig, IFormatterConfig, ILayouterConfig
    {
        public Size ImageSize { get; set; }

        public IEnumerable<string> CustomBoringWords { get; set; }

        public Font Font { get; set; }

        public Color Color { get; set; }

        public bool FrequentWordsAsHuge { get; set; }

        public float FontMultiplier { get; set; }

        public PointF CenterPoint { get; set; }

        public double AngleDelta { get; set; }

        public Config CreateConfig(Size imageSize, Color color)
        {
            return new Config
            {
                ImageSize = imageSize,
                CustomBoringWords = Enumerable.Empty<string>(),
                Color = color,
                FrequentWordsAsHuge = true,
                FontMultiplier = 7,
                CenterPoint = PointF.Empty,
                AngleDelta = 10,
            };
        }
    }
}