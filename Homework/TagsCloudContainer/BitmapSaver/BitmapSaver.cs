using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace TagsCloudContainer.BitmapSaver
{
    public class BitmapSaver : IBitmapSaver
    {
        private readonly HashSet<string> allowedExt = new()
        {
            ".png",
            ".jpeg",
            ".jpg",
            ".gif",
            ".bmp",
            ".icon"
        };

        public Result<None> Save(Bitmap bmp, string fullPathWithExt)
        {
            return CheckEmptyOrNull(fullPathWithExt)
                .Then(ValidateExtension)
                .Then(ValidateDirectory)
                .Then(bmp.Save)
                .RefineError("File saving error");
        }

        private Result<string> ValidateDirectory(string path)
        {
            var directory = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                Result.Of(() => Directory.CreateDirectory(directory).FullName)
                    .Then(_ => path);
            return Result.Ok(path);
        }

        private Result<string> ValidateExtension(string path)
        {
            var ext = Path.GetExtension(path);
            return allowedExt.Contains(ext)
                ? Result.Ok(path)
                : Result.Fail<string>($"File {path} has wrong image extension {ext}");
        }

        private Result<string> CheckEmptyOrNull(string path)
        {
            return !string.IsNullOrEmpty(path)
                ? Result.Ok(path)
                : Result.Fail<string>("Saving path is empty or null");
        }
    }
}