using System.Drawing;
using TagCloud.IntermediateClasses;

namespace TagCloud.Interfaces
{
    public interface IFontScheme
    {
        Result<Font> Process(PositionedElement element);
    }
}