using System.Drawing;
using TagCloud.IntermediateClasses;
using TagCloud.Result;

namespace TagCloud.Interfaces
{
    public interface IFontScheme
    {
        Result<Font> Process(PositionedElement element);
    }
}