using System.Drawing;
using TagCloud.IntermediateClasses;

namespace TagCloud.Interfaces
{
    public interface IColorScheme
    {
        Result<Color> Process(PositionedElement element);
    }
}