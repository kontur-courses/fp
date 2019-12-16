using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using CSharpFunctionalExtensions;

namespace TagsCloudLibrary.Writers
{
    public abstract class BuiltinFormatWriter : IImageWriter
    {
        private ImageFormat imageFormat;
        private string imageExtension;

        protected BuiltinFormatWriter(ImageFormat imageFormat, string imageExtension)
        {
            this.imageFormat = imageFormat;
            this.imageExtension = imageExtension;
        }


        public virtual Result WriteBitmapToFile(Bitmap bitmap, string fileName)
        {
            try
            {
                var extension = Path.GetExtension(fileName);
                if (extension == "")
                {
                    fileName = fileName + "." + imageExtension;
                }

                bitmap.Save(fileName, imageFormat);
            }
            catch (Exception e)
            {
                return Result.Failure(e.Message);
            }

            return Result.Ok();
        }
    }
}
