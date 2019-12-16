using System.Collections.Generic;
using System.Drawing;
using ResultOf;
using TagCloud.Models;

namespace TagCloud
{
    public interface ICloud
    {
        Result<List<TagRectangle>> GetRectangles(Graphics graphics, ImageSettings imageSettings, string path = null);
    }
}