using System.Drawing;
using System.Drawing.Imaging;
using TagsCloudGenerator.Attributes;
using TagsCloudGenerator.Interfaces;
using FailuresProcessing;

namespace TagsCloudGenerator.Savers
{
    [Factorial("PngSaver")]
    public class PngSaver : ISaver
    {
        public Result<None> SaveTo(string filePath, Bitmap bitmap)
        {
            return
                Result.Of(() => bitmap.Save(filePath, ImageFormat.Png))
                .RefineError($"Failed to save file to \'{filePath}\' path")
                .RefineError($"{nameof(PngSaver)} failure");
        }
    }
}