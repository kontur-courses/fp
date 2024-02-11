using ResultOf;

namespace TagsCloudContainer.TextTools
{
    public interface ITextReader
    {
        public static Result<string> ReadText(string filePath)
        {
            return Result<string>.Ok(string.Empty);
        }
    }
}
