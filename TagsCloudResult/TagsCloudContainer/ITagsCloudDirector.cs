using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudContainer
{
    public interface ITagsCloudDirector
    {
        Result<Bitmap> RenderWords(IEnumerable<string> words);
    }
}