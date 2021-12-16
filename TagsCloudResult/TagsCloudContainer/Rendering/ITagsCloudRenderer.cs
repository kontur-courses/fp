using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudContainer.Rendering
{
    public interface ITagsCloudRenderer
    {
        Bitmap GetBitmap(IEnumerable<WordStyle> words, Size imageSize);
    }
}