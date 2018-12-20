using System.Collections.Generic;

namespace TagsCloud.Layout
{
    public interface ITagCloudLayouter
    {
        Result<List<Tag>> GetLayout(ICollection<KeyValuePair<string, double>> words);
    }
}