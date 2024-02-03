using System.Drawing;

namespace TagCloud.ColorSelectors;

public interface IColorSelector
{
    Color PickColor();
}