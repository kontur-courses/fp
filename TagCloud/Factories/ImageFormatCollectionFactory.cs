using System.Collections.Generic;
using System.Drawing.Imaging;

namespace TagCloud.Models
{
    public static class ImageFormatCollectionFactory
    {
        public static Dictionary<string, ImageFormat> GetFormats()
        {
            return new Dictionary<string, ImageFormat>
            {
                {"jpg", ImageFormat.Jpeg},
                {"png", ImageFormat.Png},
                {"bmp", ImageFormat.Bmp}
            };
        }
    }
}