using SixLabors.ImageSharp;
using TagsCloud.Entities;
using TagsCloud.Results;
using TagsCloudVisualization;

namespace TagsCloud.Painters;

public interface IPainter
{
    ColoringStrategy Strategy { get; }
    Result<HashSet<WordTagGroup>> Colorize(HashSet<WordTagGroup> wordGroups, Color[] colors);
}