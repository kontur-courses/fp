using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudContainer.Drawing
{
    public interface IDrawer
    {
        Result<Bitmap> DrawImage(IEnumerable<Tag> tags);
    }
}