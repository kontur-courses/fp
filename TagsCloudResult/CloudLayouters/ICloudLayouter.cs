using System.Collections.Generic;
using TagsCloudResult.CloudVisualizers;
using TagsCloudResult.TextParsing.CloudParsing;

namespace TagsCloudResult.CloudLayouters
{
    public interface ICloudLayouter
    {
        IEnumerable<CloudVisualizationWord> GetWords(IEnumerable<CloudWord> cloudWords);
    }
}