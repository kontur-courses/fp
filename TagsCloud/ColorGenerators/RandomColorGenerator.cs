using System.Drawing;
using TagsCloud.Entities;


namespace TagsCloud.ColorGenerators;

public class RandomColorGenerator : IColorGenerator
{
    private static readonly Random Random = new Random();

    public Result<Color> GetTagColor(Tag tag)
    {
        var color =Color.FromArgb(Random.Next(256), Random.Next(256), Random.Next(256));
        return Result.Ok(color);
    }
}