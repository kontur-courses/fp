using System.Drawing;
using TagsCloud.Entities;

namespace TagsCloud.ColorGenerators;

public interface IColorGenerator
{
    Result<Color> GetTagColor(Tag tag);
}