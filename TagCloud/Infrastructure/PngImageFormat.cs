using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ResultOf;

namespace TagCloud.Infrastructure
{
    public class PngImageFormat : IImageFormat
    {
        public Result<None> SaveImage(Bitmap bitmap, string filePath)
        {
            try
            {
                bitmap.Save($"{filePath}.png");
                return Result.Ok();
            }
            catch (ExternalException e)
            {
                return Result.Fail<None>(e.Message).RefineError($"Failed to save picture to {filePath}.png");
            }
            
        }
    }
}
