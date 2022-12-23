using ResultOf;

namespace TagsCloudVisualization.TextFormatters
{
    public interface IWordFilter
    {
        Result<bool> IsPermitted(string word);
    }
}