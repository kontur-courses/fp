using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using CSharpFunctionalExtensions;

namespace TagsCloudLibrary.Writers
{
    abstract class AbstractWriter : IImageWriter
    {
        public abstract Result WriteBitmapToFile(Bitmap bitmap, string fileName);
    }
}
