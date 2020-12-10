using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text.RegularExpressions;
using ResultOf;
using TagsCloudContainer.TagsCloudVisualization.Interfaces;

namespace TagsCloudContainer.TagsCloudVisualization
{
    public class BitmapSaver : IBitmapSaver
    {
        public void SaveBitmapToDirectory(Bitmap imageBitmap, string savePath)
        {
            Result.Ok(savePath)
                .Then(PathInRightFormat)
                .OnFail(e => throw new ArgumentException(e));

            using (imageBitmap)
            {
                imageBitmap.Save(savePath, ImageFormat.Png);
            }
        }

        private static Result<string> PathInRightFormat(string path)
        {
            var separator = Path.DirectorySeparatorChar;
            var pattern = $@"((?:[^\{separator}]*\{separator})*)(.*[.].+)";
            var match = Regex.Match(path, pattern);
            var directoryPath = match.Groups[1].ToString();

            return Directory.Exists(directoryPath) && match.Groups[2].Success
                ? Result.Ok(path)
                : Result.Fail<string>("wrong path format");
        }
    }
}