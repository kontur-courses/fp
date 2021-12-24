using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace TagsCloudContainerCore;

// ReSharper disable once ClassNeverInstantiated.Global
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public class TagToRender
{
    public string Value { get; }

    public int ColorHex { get; }

    public float FontSize { get; }

    public string FontName { get; }

    public Rectangle LimitingRectangle { get; }

    // ReSharper disable once UnusedMember.Global
    public Point Location => LimitingRectangle.Location;

    public TagToRender(Rectangle limitingRectangle, string value, int colorHex, float fontSize, string fontName)
    {
        Value = value;
        ColorHex = colorHex;
        FontSize = fontSize;
        FontName = fontName;
        LimitingRectangle = limitingRectangle;
    }
}