using System.Drawing;
using TagsCloud.ResultPattern;

namespace TagsCloud.Visualization
{
    public interface IImageHolder
    {
        Result<Size> GetImageSize();
        Result<Graphics> StartDrawing();
        void RecreateImage(ImageSettings settings);
        Result<None> SaveImage(string fileName);
    }
}