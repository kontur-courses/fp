using System.Collections.Generic;
using System.Drawing;
using TagsCloud.TagGenerators;
using TagsCloud.ErrorHandling;

namespace TagsCloud.Interfaces
{
    public interface ICloudDrawer
    {
        Result<Image> Paint(IEnumerable<(Tag tag, Rectangle position)> resultTagCloud, Size imageSize, Color backgroundColor, int widthOfBorder = 0);
    }
}
