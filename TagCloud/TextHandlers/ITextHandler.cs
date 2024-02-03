namespace TagCloud.TextHandlers;

public interface ITextHandler
{
    Result<Dictionary<string, int>> Handle();
}