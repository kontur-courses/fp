using System.IO;

namespace TagsCloudGenerator.Tools
{
    public static class PathHelper
    {
        public static string GetFileExtension(string path)
        {
            var extension = Path.GetExtension(path);

            return extension?.Substring(1);
        }
    }
}