using ResultOf;

namespace TagCloud.Common.WeightCounter;

public interface IWeightCounter
{
    Result<Dictionary<string, int>> CountWeights(IEnumerable<string> words);
}