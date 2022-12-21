using ResultOf;

namespace TagCloud.Common.WeightCounter;

public class SimpleWeightCounter : IWeightCounter
{
    public Result<Dictionary<string, int>> CountWeights(IEnumerable<string> words)
    {
        var list = words.ToList();
        if (!list.Any())
        {
            return Result.Fail<Dictionary<string, int>>("There was no words inside WeightCounter");
        }

        return list
            .GroupBy(x => x)
            .ToDictionary(x => x.Key, x => x.Count())
            .OrderByDescending(x => x.Value)
            .ToDictionary(x => x.Key, x => x.Value).AsResult();
    }
}