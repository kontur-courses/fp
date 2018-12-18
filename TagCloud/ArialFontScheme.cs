using System.Drawing;
using TagCloud.Interfaces;
using TagCloud.IntermediateClasses;
using TagCloud.Result;

namespace TagCloud
{
    public class ArialFontScheme : IFontScheme
    {
        public const string FamilyName = "Arial";
        public const int Size = 16;

        public Result<Font> Process(PositionedElement element)
        {
            return Result.Result.Of(() => new Font(FamilyName, Size));
        }
    }
}