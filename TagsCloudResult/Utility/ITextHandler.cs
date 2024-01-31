using ResultSharp;

namespace TagsCloudResult.Utility;

public interface ITextHandler
{
    Result<string> ReadText(string filePath);
}