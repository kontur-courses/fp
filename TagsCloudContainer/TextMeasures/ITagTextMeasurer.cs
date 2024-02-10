
using System.Drawing;

namespace TagsCloudContainer.TextMeasures;

public interface ITagTextMeasurer
{
    public Result<Size> Measure(string text);
}