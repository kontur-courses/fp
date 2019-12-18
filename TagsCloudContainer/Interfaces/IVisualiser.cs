using System.Drawing;
using ResultOf;
using TagsCloudContainer.Layouter;

namespace TagsCloudContainer.Interfaces
{
    public interface IVisualiser
    {
        Result<Bitmap> DrawRectangles(ICloudLayouter ccl, (string, Size)[] arr);
    }
}