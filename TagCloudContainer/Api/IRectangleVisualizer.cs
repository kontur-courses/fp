using System.Drawing;

namespace TagCloudContainer.Api
{
    [CliRole]
    public interface IRectangleVisualizer
    {
        Image CreateImageWithRectangles();
    }
}