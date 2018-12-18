using System.Collections.Generic;
using ResultOf;


namespace TagsCloudVisualization.Preprocessing
{
    public interface IFilter
    {
        IEnumerable<string> FilterWords(IEnumerable<string> words);
    }
}
