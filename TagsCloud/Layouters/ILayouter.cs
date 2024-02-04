using System.Drawing;
using TagsCloud.Entities;
using TagsCloud.Result;

namespace TagsCloud.Layouters;

public interface ILayouter
{
    public Result<IEnumerable<Tag>> GetTagsCollection();
    public Result<None> CreateTagCloud(Dictionary<string, int> tagsDictionary);

    public Result<Size> GetImageSize();
}