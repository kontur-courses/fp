using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class ImageSaver : IImageSaver
    {
        public Result<FileSaveResult> WriteToFile(string fileName, Image bitmap)
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
