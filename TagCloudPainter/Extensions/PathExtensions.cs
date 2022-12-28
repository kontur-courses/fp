using TagCloudPainter.ResultOf;

namespace TagCloudPainter.Extensions
{
    public static class PathExtensions
    {
        public static Result<string> ValidatePath(this string path, string extension)
        {
            if (!path.EndsWith(extension))
                return Result.Fail<string>($"file is not in {extension} format");

            if (!File.Exists(path))
                return Result.Fail<string>($"path {path} does not exist ");

            return Result.Ok(path);
        }
    }
}
