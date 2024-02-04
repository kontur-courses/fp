using ResultOf;

namespace TagsCloudContainer.TextTools
{
    public interface ITextReader
    {
        public Result<string> ReadText(string filePath);
    }
}
