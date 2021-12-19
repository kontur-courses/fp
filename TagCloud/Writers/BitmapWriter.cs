using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;

namespace TagCloud.Writers
{
    public class BitmapWriter : IFileWriter
    {
        public void Write(Bitmap bitmap, string filename)
        {
            using (bitmap)
            {
                var extension = GetExtensionsFromFileName(filename);
                Console.WriteLine(extension);
                var format = GetImageFormatByExtension(extension);
                Console.WriteLine(format);
                var env = Environment.CurrentDirectory + "\\";
                bitmap.Save(env + filename, format);
            }
        }

        private static string GetExtensionsFromFileName(string filename)
        {
            var lastDotIndex = filename.LastIndexOf('.');
            return filename[(lastDotIndex + 1)..];
        }

        private ImageFormat GetImageFormatByExtension(string extension)
        {
            return (ImageFormat)typeof(ImageFormat)
                .GetProperty(extension, BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase)
                .GetValue(extension, null);
        }
    }
}
