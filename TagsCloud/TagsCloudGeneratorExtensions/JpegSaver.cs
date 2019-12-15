using System.Drawing;
using System.Drawing.Imaging;
using TagsCloudGenerator.Attributes;
using TagsCloudGenerator.Interfaces;
using FailuresProcessing;

namespace TagsCloudGeneratorExtensions
{
    [Factorial("JpegSaver")]
    public class JpegSaver : ISaver
    {
        public Result<None> SaveTo(string filePath, Bitmap bitmap)
        {
            return
                Result.Of(() => bitmap.Save(filePath, ImageFormat.Jpeg))
                .RefineError($"Failed to save file to \'{filePath}\' path")
                .RefineError($"{nameof(JpegSaver)} failure");
        }
    }
}