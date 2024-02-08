using System.Drawing;
using TagsCloud.Entities;

namespace TagsCloud.Layouters;

public interface ILayouter
{
    public Result<Cloud> CreateTagsCloud(Dictionary<string, Font> tagsDictionary);
}