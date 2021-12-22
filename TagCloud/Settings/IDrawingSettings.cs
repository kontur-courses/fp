using System.Collections.Generic;

namespace TagCloud.Settings
{
    public interface IDrawingSettings : ITagCreatorSettings
    {
        public IEnumerable<string> PenColors { get; }
        public string BackgroundColor { get; }
        public int Width { get; }
        public int Height { get; }
        public string AlgorithmName { get; }
    }
}
