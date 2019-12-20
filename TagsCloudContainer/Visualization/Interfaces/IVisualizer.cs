using System.Collections.Generic;
using TagsCloudContainer.ResultInfrastructure;

namespace TagsCloudContainer.Visualization.Interfaces
{
    public interface IVisualizer
    {
        Result Visualize(IEnumerable<WordRectangle> wordRectangles, string path);
    }
}