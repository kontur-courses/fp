using ResultOf;

namespace TagCloud2.Image
{
    public interface IColoredCloudToImageConverter
    {
        Result<System.Drawing.Image> GetImage(IColoredCloud cloud, int xSize, int ySize);
    }
}
