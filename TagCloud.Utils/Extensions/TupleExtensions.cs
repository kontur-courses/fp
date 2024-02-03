using Aspose.Drawing;
using TagCloud.Utils.ResultPattern;

namespace TagCloud.Utils.Extensions;

public static class TupleExtensions
{
    public static Result<Color> ParseColor(this (int red, int green, int blue) from)
    {
        return Result.Of(() => Color.FromArgb(255, from.red, from.green, from.blue));
    }
}