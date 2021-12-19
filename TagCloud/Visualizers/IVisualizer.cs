using System.Collections.Generic;
using System.Drawing;
using TagCloud.Creators;
using TagCloud.ResultMonad;

namespace TagCloud.Visualizers
{
    public interface IVisualizer
    {
        Result<Bitmap> DrawCloud(IEnumerable<Tag> tags,
            ITagColoringFactory tagColoringAlgorithm);
    }
}