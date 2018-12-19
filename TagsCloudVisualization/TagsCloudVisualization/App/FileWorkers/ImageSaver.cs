using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class ImageSaver : IImageSaver
    {
        public Result<None> WriteToFile(string fileName, Image bitmap)
        {
            try
            {
                bitmap.Save(fileName, bitmap.RawFormat);
                return new None();
            }
            catch (Exception e)
            {
                return Result.Fail<None>(e.Message);
            }
        }
    }
}
