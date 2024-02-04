using System.Drawing;
using TagsCloudVisualization.Settings;

namespace TagsCloudVisualization.ColorGenerators;

public class DefaultColorGenerator : IColorGenerator
{
    private readonly Color color;
    private readonly bool isMatch;

    public DefaultColorGenerator(ColorGeneratorSettings settings)
    {
        isMatch = IsCanGetColorFromString(settings.Color);
        if (isMatch)
            color = Color.FromName(settings.Color);
    }

    public Color GetColor()
    {
        return color;
    }

    public bool IsCanGetColorFromString(string color)
    {
        if (Enum.IsDefined(typeof(KnownColor), color))
            return true;
        return false;
    }

    public bool Match()
    {
        return isMatch;
    }
}