using System.Collections.Generic;
using ResultMonad;

namespace TagsCloudDrawer.ImageCreator
{
    public interface IImageCreator
    {
        Result<None> Create(string filename, IEnumerable<Result<IDrawable>> tags);
    }
}