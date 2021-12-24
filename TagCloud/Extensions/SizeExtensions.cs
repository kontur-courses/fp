using System;
using System.Drawing;

namespace TagCloud.Extensions;

public static class SizeExtensions
{
    public static SizeF Combine(this SizeF size, SizeF other)
    {
        return new SizeF(Math.Max(size.Width, other.Width), Math.Max(size.Height, other.Height));
    }
}