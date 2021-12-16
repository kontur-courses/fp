using System.Drawing;

namespace App.Infrastructure
{
    public interface ICloudGenerator
    {
        Result<Bitmap> GenerateCloud();
    }
}