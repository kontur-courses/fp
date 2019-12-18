using System.Collections.Generic;
using System.Drawing;
using TagsCloud.ErrorHandling;
using TagsCloud.TagGenerators;

namespace TagsCloud.Interfaces
{
    public interface ICloudDrawer
    {
        Result<Image> Paint(IEnumerable<(Tag tag, Rectangle position)> resultTagCloud, Size imageSize,
            Color backgroundColor, int widthOfBorder = 0);
    }
}