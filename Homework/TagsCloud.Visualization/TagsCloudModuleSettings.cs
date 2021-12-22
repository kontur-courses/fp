using System;
using System.Drawing;
using TagsCloud.Visualization.ColorGenerators;
using TagsCloud.Visualization.FontFactories;
using TagsCloud.Visualization.ImagesSavers;

namespace TagsCloud.Visualization
{
    public class TagsCloudModuleSettings
    {
        public Point Center { get; init; }
        public string InputWordsFile { get; init; }
        public string BoringWordsFile { get; init; }
        public Type LayouterType { get; init; }
        public IColorGenerator ColorGenerator { get; init; }
        public SaveSettings SaveSettings { get; init; }
        public FontSettings FontSettings { get; init; }
    }
}