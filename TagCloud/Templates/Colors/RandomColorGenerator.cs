using System;
using System.Drawing;

namespace TagCloud.Templates.Colors;

public class RandomColorGenerator : IColorGenerator
{
    private static readonly Random Random = new();
    public Color GetColor(string word)
    {
        var r = Random.Next(255);
        var g = Random.Next(255);
        var b = Random.Next(255);
        return Color.FromArgb(r, g, b);
    }
}