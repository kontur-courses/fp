using System.Drawing;
using TagsCloud.Entities;
using TagsCloud.Result;

namespace TagsCloud.ColorGenerators;

public interface IColorGenerator
{
    Result<Color> GetTagColor(Tag tag);
}