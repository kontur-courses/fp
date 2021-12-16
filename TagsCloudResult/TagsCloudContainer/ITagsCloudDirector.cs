using System.Collections.Generic;
using System.Drawing;
using TagsCloudContainer.Results;

namespace TagsCloudContainer
{
    public interface ITagsCloudDirector
    {
        Result<Bitmap> RenderWords(IEnumerable<string> words);
    }
}