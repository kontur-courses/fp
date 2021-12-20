using System;
using System.Drawing;
using TagsCloud.Visualization.ContainerVisitor;
using TagsCloud.Visualization.FontFactory;
using TagsCloud.Visualization.ImagesSavers;

namespace TagsCloud.Visualization
{
    public class TagsCloudModuleSettings
    {
        public Point Center { get; init; }
        public string InputWordsFile { get; init; }
        public string BoringWordsFile { get; init; }
        public Type LayouterType { get; init; }
        public IContainerVisitor LayoutVisitor { get; init; }
        public SaveSettings SaveSettings { get; init; }
        public FontSettings FontSettings { get; init; }
    }
}