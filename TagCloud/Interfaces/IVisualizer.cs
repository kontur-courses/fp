using System.Collections.Generic;
using System.Drawing;
using TagCloud.IntermediateClasses;

namespace TagCloud.Interfaces
{
    public interface IVisualizer
    {
        Result<Image> Visualize(IEnumerable<PositionedElement> objects);
    }
}