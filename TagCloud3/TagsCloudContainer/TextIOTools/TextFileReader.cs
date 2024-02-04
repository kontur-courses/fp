using ResultOf;

namespace TagsCloudContainer.TextTools
{
    public class TextFileReader : ITextReader
    {
        public Result<string> ReadText(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return Result.Fail<string>($"File {filePath} not exist");
            }
            return Result.Ok<string>(File.ReadAllText(filePath));


        }
    }
}
