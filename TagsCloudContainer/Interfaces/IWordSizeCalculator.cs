using ResultOfTask;
using System.Drawing;

namespace TagsCloudContainer.Interfaces;

public interface IWordSizeCalculator
{
    public Result<Dictionary<string, Font>> CalculateSize(Dictionary<string, int> input,
        ICustomOptions options);
}