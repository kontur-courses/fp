using ResultOf;
using System.Drawing;
using TagsCloudContainer.Registrations;

namespace TagsCloudContainer.Abstractions;

public interface IVisualizer : IService
{
    Result<Bitmap> GetBitmap();
}