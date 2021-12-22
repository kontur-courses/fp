﻿using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;

namespace TagCloud.Writers
{
    public class BitmapWriter : IFileWriter
    {
        public void Write(Bitmap bitmap, string filename, string extension, string targetDirectory)
        {
            var format = GetImageFormatByExtension(extension);
            var env = targetDirectory + "\\";
            bitmap.Save(env + filename, format);
        }

        private ImageFormat GetImageFormatByExtension(string extension)
        {
            return (ImageFormat)typeof(ImageFormat)
                .GetProperty(extension, BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase)
                .GetValue(extension, null);
        }
    }
}
