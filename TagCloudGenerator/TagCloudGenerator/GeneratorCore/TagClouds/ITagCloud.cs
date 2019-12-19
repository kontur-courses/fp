using System.Drawing;
using TagCloudGenerator.GeneratorCore.CloudLayouters;
using TagCloudGenerator.ResultPattern;

namespace TagCloudGenerator.GeneratorCore.TagClouds
{
    public interface ITagCloud
    {
        Result<Bitmap> CreateBitmap(string[] cloudStrings, ICloudLayouter cloudLayouter, Size bitmapSize);
    }
}