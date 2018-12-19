using System.Collections.Generic;

namespace TagsCloud
{
    public interface IGraphics
    {
        Result<None> Save(IReadOnlyCollection<Tag> words);
    }
}