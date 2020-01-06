using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagCloud.Models
{
    public static class ImageFormatCollectionFactory
    {
        public static Dictionary<string, ImageFormat> GetFormats()
        {
            return new Dictionary<string, ImageFormat>()
            {
                {"jpg", ImageFormat.Jpeg},
                {"png", ImageFormat.Png },
                {"bmp", ImageFormat.Bmp }
            };
        }
    }
}
