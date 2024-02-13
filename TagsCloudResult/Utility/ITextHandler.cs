namespace TagsCloudResult.Utility;

public interface ITextHandler
{
    MyResult<string> ReadText(string filePath);
}