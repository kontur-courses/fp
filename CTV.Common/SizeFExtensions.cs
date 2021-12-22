using System;
using System.Drawing;

namespace CTV.Common
{
    public static class SizeFExtensions
    {
        public static Size ToCeilSize(this SizeF sizeF)
        {
            return new Size(
                (int) Math.Ceiling(sizeF.Width),
                (int) Math.Ceiling(sizeF.Height));
        }
    }
}