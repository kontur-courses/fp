using System.Collections.Generic;
using TagCloud.Creators;
using TagCloud.ResultMonad;

namespace TagCloud.Layouters
{
    public interface ICloudLayouter
    {
        Result<IEnumerable<Tag>> PutTags(IEnumerable<Tag> tags);
    }
}
