using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using CSharpFunctionalExtensions;

namespace TagsCloudLibrary.Writers
{
    public class JpegWriter : BuiltinFormatWriter
    {
        public JpegWriter() : base(ImageFormat.Jpeg, "jpeg") { }
    }
}
