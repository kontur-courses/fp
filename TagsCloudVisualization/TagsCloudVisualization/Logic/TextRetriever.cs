using System.IO;
using System.Text;
using ErrorHandler;

namespace TagsCloudVisualization.Logic
{
    public static class TextRetriever
    {
        public static readonly Encoding DefaultEncoding = Encoding.UTF8;
        public static Result<string> RetrieveTextFromFile(string path, Encoding encoding)
        {
            if (path == null)
                return Result.Fail<string>("Path is null");
            if (!File.Exists(path))
                return Result.Fail<string>($"Path {path} doesn't exist");
            var text = File.ReadAllText(path, encoding);
            return text;
        }

        public static Result<string> RetrieveTextFromFile(string path) =>
            RetrieveTextFromFile(path, DefaultEncoding);
    }
}