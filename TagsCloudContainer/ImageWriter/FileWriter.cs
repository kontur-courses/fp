using System.IO;
using TagsCloudContainer.ResultOf;

namespace TagsCloudContainer.ImageWriter
{
    public class FileWriter : IImageWriter
    {
        public Result<None> Write(byte[] image, string pathToSave)
        {
            return Result.OfAction(() => File.WriteAllBytes(pathToSave, image),
                $"Failed to write image to file {pathToSave}");
        }
    }
}