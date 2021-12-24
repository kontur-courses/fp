using System.Collections.Generic;
using System.Drawing;
using TagCloud.settings;

namespace TagCloud.visual
{
    public interface ICloudVisualizer
    {
        Result<ICloudVisualizer> InitializeCloud(IEnumerable<string> words);
        Result<Image> GetImage(IDrawSettings drawSettings);
    }
}