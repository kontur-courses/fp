using System.Collections.Generic;
using TagsCloudVisualization.ResultOf;

namespace TagsCloudVisualization.Visualization.Configurator
{
    public interface IVisualizingConfigurator
    {
        public Result<IEnumerable<IVisualizingToken>> Configure(IEnumerable<string> visualizingValues);
    }
}