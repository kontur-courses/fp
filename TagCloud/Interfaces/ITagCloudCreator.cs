using System.Drawing;
namespace TagCloud;

public interface ITagCloudCreator
{
    Result<Bitmap> GetCloud();
}