using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using TagsCloudVisualization;

namespace Cloud.ClientUI
{
    public class PngSaver : ISaver
    {
        public Result<None> SaveImage(Bitmap bitmap, string fileName)
        {
            return CheckValidFileName(fileName)
                .Then(_ =>
                {
                    var path = $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}{fileName}.png";
                    bitmap.Save(path, ImageFormat.Png);
                    return Result.Ok();
                });

        }

        private static Result<string> CheckValidFileName(string name)
        {
            var invalidSpecialCharacters = "/\\:*?\"<>|".ToCharArray();
            return name.Any(letter => invalidSpecialCharacters.Contains(letter)) 
                ? Result.Fail<string>("File name contains invalid characters") 
                : Result.Ok(name);
        }
    }
}