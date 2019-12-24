using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudContainer
{
    public class PictureInfo
    {
        public PictureInfo(string fileName, string format="png")
        {
            FileName = fileName;
            Format = availableFormats[format];
        }

        public string FileName
        {
            get { return fileName; }
            set
            {
                if (value.Length == 0)
                    throw new ArgumentException("Text file name must be not empty.\n" +
                                                "Change it.");
                fileName = value;
            }
        }

        public readonly ImageFormat Format;
        private string fileName;
        public static readonly Dictionary<string, ImageFormat> availableFormats =
            new Dictionary<string, ImageFormat>()
            {
                {"bmp", ImageFormat.Bmp},
                {"wmf", ImageFormat.Wmf},
                {"emf", ImageFormat.Emf},
                {"exif", ImageFormat.Exif},
                {"gif", ImageFormat.Gif},
                {"icon", ImageFormat.Icon},
                {"jpeg", ImageFormat.Jpeg},
                {"png", ImageFormat.Png},
                {"tiff", ImageFormat.Tiff}
            };
    }
}