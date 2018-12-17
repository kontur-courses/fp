using System.Collections.Generic;
using ResultOf;

namespace TagsCloudVisualization.Layouter
{
    public interface IWordsCloudBuilder
    {
        int Radius { get; }
        Result<IEnumerable<Word>> Build();
    }
}