using ResultOf;

namespace TagsCloudContainer.TextTools
{
    public class TextFileReader : ITextReader
    {
        public static Result<string> ReadText(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return Result<string>.Fail($"File path can't be empty");
            }

            if (!File.Exists(filePath))
            {
                return Result<string>.Fail($"File {Path.GetFullPath(filePath)} not found");
            }

            return Result<string>.Ok(File.ReadAllText(filePath));
        }
    }
}
