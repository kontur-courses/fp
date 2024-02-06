using ResultOf;

namespace TagsCloudContainer.TextTools
{
    public class TextFileReader : ITextReader
    {
        public Result<string> ReadText(string filePath)
        {
            if(filePath == null)
            {
                return Result.Fail<string>($"File path can't be empty");
            }

            if (!File.Exists(filePath))
            {
                return Result.Fail<string>($"File {Path.GetFullPath(filePath)} not found");
            }

            return Result.Ok<string>(File.ReadAllText(filePath));
        }
    }
}
