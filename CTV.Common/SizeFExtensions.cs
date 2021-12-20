using System.Drawing;

namespace CTV.Common
{
    public static class SizeFExtensions
    {
        public static Size ToRoundSize(this SizeF sizeF)
        {
            return Size.Round(sizeF);
        }
    }
}