using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using CSharpFunctionalExtensions;

namespace TagsCloudLibrary.Writers
{
    public class BmpWriter : BuiltinFormatWriter
    {
        public BmpWriter() : base(ImageFormat.Bmp, "bmp") { }
    }
}
