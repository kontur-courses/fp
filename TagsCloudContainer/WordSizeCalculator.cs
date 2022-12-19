using System.Drawing;
using TagsCloudContainer.Interfaces;
using Result;

namespace TagsCloudContainer;

public class WordSizeCalculator : IWordSizeCalculator
{
    public Result<Dictionary<string, Font>> CalculateSize(Result<Dictionary<string, int>> input,
        Result<ICustomOptions> options)
    {
        var result = new Dictionary<string, Font>(input.Value.Count);
        var max = input.Value.First().Value;
        var min = input.Value.Last().Value;

        var fontMax = options.Value.MaxTagSize;
        var fontMin = options.Value.MinTagSize;

        foreach (var pair in input.Value)
        {
            var size = pair.Value == min
                ? fontMin
                : (pair.Value / (double)max) * (fontMax - fontMin) + fontMin;
            result.Add(pair.Key, new Font(options.Value.Font, (int)size));
        }

        return new Result<Dictionary<string, Font>>(result);
    }
}