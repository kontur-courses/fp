using System.Drawing;
using ErrorHandler;

namespace TagsCloudVisualization.Services
{
    public interface IImageHolder
    {
        Result<None> SetImage(Bitmap image);

        Result<Image> GetImage();
    }
}