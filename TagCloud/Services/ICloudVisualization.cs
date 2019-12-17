using System.Collections.Generic;
using System.Drawing;
using ResultOf;
using TagCloud.Models;

namespace TagCloud
{
    public interface ICloudVisualization
    {
        Dictionary<string, Palette> PaletteDictionary { get; }
        Result<Bitmap> GetAndDrawRectangles(ImageSettings imageSettings, string path);
    }
}