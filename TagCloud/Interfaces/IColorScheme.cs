using System.Drawing;
using TagCloud.IntermediateClasses;
using TagCloud.Result;

namespace TagCloud.Interfaces
{
    public interface IColorScheme
    {
        Result<Color> Process(PositionedElement element);
    }
}