using Result;

using System.Drawing;

namespace TagsCloudContainer.Interfaces;

public interface IWordSizeCalculator
{
    public Result<Dictionary<string, Font>> CalculateSize(Result<Dictionary<string, int>> input,
        Result<ICustomOptions> options);
}