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
            var separator = Path.DirectorySeparatorChar;
            var pattern = $@"((?:[^\{separator}]*\{separator})*)(.*[.].+)";
            var match = Regex.Match(savePath, pattern);

            Result.Ok(match)
                .Then(ValidateMatch)
                .Then(ValidateDirectoryPath)
                .Then(ValidateFileName)
                .OnFail(e => throw new ArgumentException(e));

            Result.Ok(imageBitmap)
                .Then(ValidateBitmap)
                .OnFail(e => throw new ArgumentException(e))
                .Then(x =>
                {
                    using (x)
                    {
                        x.Save(savePath, ImageFormat.Png);
                    }
                });
        }

        private Result<Match> ValidateMatch(Match match)
        {
            return Validate(match, x => !match.Success, $"Wrong path format: {match.Value}");
        }

        private Result<Bitmap> ValidateBitmap(Bitmap bitmap)
        {
            return Validate(bitmap, x => x == null, "Bitmap is null");
        }

        private Result<Match> ValidateDirectoryPath(Match match)
        {
            var path = match.Groups[1].ToString();
            return Validate(match, x => !Directory.Exists(path), $"Directory {path} doesnt exists");
        }

        private Result<Match> ValidateFileName(Match match)
        {
            var fileName = match.Groups[2].ToString();
            return Validate(match, x => !match.Groups[2].Success, $"Wrong file name: {fileName}");
        }

        private Result<T> Validate<T>(T obj, Func<T, bool> predicate, string exception)
        {
            return predicate(obj)
                ? Result.Fail<T>(exception)
                : Result.Ok(obj);
        }
    }
}