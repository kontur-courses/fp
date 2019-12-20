using System.Collections.Generic;
using TagsCloudContainer.ResultInfrastructure;

namespace TagsCloudContainer.Visualization.Interfaces
{
    public interface IVisualizer
    {
        Result<None> Visualize(IEnumerable<WordRectangle> wordRectangles, string path);
    }
}