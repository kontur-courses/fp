using System.IO;
using TagCloud.ResultMonade;

namespace TagCloud.FileReader
{
    public class TxtFileReader : IFileReader
    {
        public Result<string> ReadAllText(string filePath)
        {
            if (!File.Exists(filePath))
                return Result.Fail<string>($"File {filePath} doesn't exist");

            if (Path.GetExtension(filePath) != ".txt")
                return Result.Fail<string>($"File {filePath} isn't format .txt");

            var text = Result.Of(() => File.ReadAllText(filePath));

            if (text.IsSuccess && text.Value.Length == 0)
                return Result.Fail<string>($"File {filePath} is empty");

            return text;
        }
    }
}
