using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using CSharpFunctionalExtensions;

namespace TagsCloudLibrary.Writers
{
    public class PngWriter : BuiltinFormatWriter
    {
        public PngWriter() : base(ImageFormat.Png, "png") { }
    }
}