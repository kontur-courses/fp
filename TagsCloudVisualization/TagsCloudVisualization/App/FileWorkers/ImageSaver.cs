using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public static class ImageSaver
    {
        public static Result<FileSendResult> WriteToFile(string fileName, Image bitmap)
        {
            try
            {
                bitmap.Save(fileName, bitmap.RawFormat);
                return Result.Ok(new FileSendResult());
            }
            catch (Exception e)
            {
                return Result.Fail<FileSendResult>(e.Message);
            }
        }
    }

    public class FileSendResult
    {

    }
}
