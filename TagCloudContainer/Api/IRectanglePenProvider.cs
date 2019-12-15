using System.Drawing;

namespace TagCloudContainer.Api
{
    [CliRole]
    public interface IRectanglePenProvider
    {
        Pen CreatePenForRectangle(Rectangle rectangle);
    }
}