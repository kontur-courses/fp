using System.IO;
using System.Text;
using ErrorHandler;

namespace TagsCloudVisualization.Logic
{
    public static class TextRetriever
    {
        public static Result<string> RetrieveTextFromFile(string path)
        {
            if (path == null)
                return Result.Fail<string>("Path is null");
            if (!File.Exists(path))
                return Result.Fail<string>("Path to text file doesn't exist");
            var text = File.ReadAllText(path, Encoding.UTF8);
            return text;
        }
    }
}