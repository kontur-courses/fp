using System.IO;
using FunctionalTools;

namespace TagsCloudGenerator.Tools
{
    public static class PathHelper
    {
        public static Result<string> GetFileExtension(string path)
        {
            return Result
                .Of(() => Path.GetExtension(path)?.Substring(1))
                .RefineError($"Can't get extension of {path}");
        }
    }
}