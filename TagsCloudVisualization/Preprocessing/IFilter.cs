using System.Collections.Generic;
using ResultOf;


namespace TagsCloudVisualization.Preprocessing
{
    public interface IFilter
    {
        Result<IEnumerable<string>> FilterWords(IEnumerable<string> words);
    }
}
