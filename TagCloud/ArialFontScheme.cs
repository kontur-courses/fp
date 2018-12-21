using System.Drawing;
using TagCloud.Interfaces;
using TagCloud.IntermediateClasses;

namespace TagCloud
{
    public class ArialFontScheme : IFontScheme
    {
        public const string FamilyName = "Arial";
        public const int Size = 16;

        public Result<Font> Process(PositionedElement element)
        {
            return Result.Of(() => new Font(new FontFamily(FamilyName), Size));
        }
    }
}