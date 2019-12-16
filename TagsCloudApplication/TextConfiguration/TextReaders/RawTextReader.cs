using System.IO;

namespace TextConfiguration.TextReaders
{
    public class RawTextReader : ITextReader
    {
        public Result<string> ReadText(string filePath)
        {
            if (filePath is null || !File.Exists(filePath))
                return Result.Fail<string>($"Couldn't read file. Incorrect file path: {filePath}");

            return File.ReadAllText(filePath);
        }
    }
}
