using System.Collections.Generic;

namespace TagsCloud
{
    public interface ITagCloudLayouter
    {
        Result<IReadOnlyCollection<Tag>> GetLayout(ICollection<KeyValuePair<string, double>> words);
    }
}