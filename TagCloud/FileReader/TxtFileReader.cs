using System.IO;
using TagCloud.ResultMonade;

namespace TagCloud.FileReader
{
    public class TxtFileReader : IFileReader
    {
        public Result<string> ReadAllText(string filePath)
        {
            if (!filePath.EndsWith(".txt"))
                return Result.Fail<string>($"File {filePath} isn't format .txt");

            if (!File.Exists(filePath))
                return Result.Fail<string>($"File {filePath} doesn't exist");

            var text = Result.Of(() => File.ReadAllText(filePath));

            if (text.IsSuccess && text.Value.Length == 0)
                return Result.Fail<string>($"File {filePath} is empty");

            return text;
        }
    }
}
