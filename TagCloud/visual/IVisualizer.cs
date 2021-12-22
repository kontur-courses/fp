using System.Collections.Generic;
using System.Drawing;
using TagCloud.settings;

namespace TagCloud.visual
{
    public interface IVisualizer
    {
        Result<IVisualizer> InitializeCloud(List<string> words);
        Result<Image> GetImage(IDrawSettings drawSettings);
    }
}