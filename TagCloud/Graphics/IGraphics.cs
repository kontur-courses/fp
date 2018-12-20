using System.Collections.Generic;
using TagsCloud.Layout;

namespace TagsCloud.Graphics
{
    public interface IGraphics
    {
        Result<None> Save(IReadOnlyCollection<Tag> words);
    }
}