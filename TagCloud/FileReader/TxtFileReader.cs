using System.IO;
using ResultOf;

namespace TagCloud.FileReader
{
    public class TxtFileReader : IFileReader
    {
        public Result<string> ReadAllText(string filePath)
        {
            if (!File.Exists(filePath))
                return new Result<string>($"File {filePath} doesn't exist");

            if (Path.GetExtension(filePath) != ".txt")
                return new Result<string>($"File {filePath} has invalid format");

            var text = Result.Of(() => File.ReadAllText(filePath));

            if (text.IsSuccess && text.Value.Length == 0)
                return new Result<string>($"File {filePath} is empty");

            return text;
        }
    }
}
