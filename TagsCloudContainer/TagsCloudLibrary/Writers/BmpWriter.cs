using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using CSharpFunctionalExtensions;

namespace TagsCloudLibrary.Writers
{
    public class BmpWriter : IImageWriter
    {
        public Result WriteBitmapToFile(Bitmap bitmap, string fileName)
        {
            try
            {
                var extension = Path.GetExtension(fileName);
                if (extension == "")
                {
                    fileName = fileName + ".bmp";
                }

                bitmap.Save(fileName, ImageFormat.Bmp);
            }
            catch (Exception e)
            {
                return Result.Failure(e.Message);
            }

            return Result.Ok();
        }
    }
}
