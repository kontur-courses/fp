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
            var namesFormatsToSave = new Dictionary<string, ImageFormat>();
            namesFormatsToSave["jpg"] = ImageFormat.Jpeg;
            namesFormatsToSave["png"] = ImageFormat.Png;
            namesFormatsToSave["bmp"] = ImageFormat.Bmp;
            return namesFormatsToSave;
        }
    }
}
