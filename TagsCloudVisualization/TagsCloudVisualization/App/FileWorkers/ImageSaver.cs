using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public static class ImageSaver
    {
        public static Result<FileSaveResult> WriteToFile(string fileName, Image bitmap)
        {
            try
            {
                bitmap.Save(fileName, bitmap.RawFormat);
                return new FileSaveResult();
            }
            catch (Exception e)
            {
                return Result.Fail<FileSaveResult>(e.Message);
            }
        }
    }
}
