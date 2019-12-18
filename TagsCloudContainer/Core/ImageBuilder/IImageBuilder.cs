using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudContainer.Core.ImageBuilder
{
    interface IImageBuilder
    {
        Bitmap Build(IEnumerable<Tag> tags, Size size);
    }
}