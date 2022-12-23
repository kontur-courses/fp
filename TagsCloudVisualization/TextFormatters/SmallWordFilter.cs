using ResultOf;

namespace TagsCloudVisualization.TextFormatters
{
    public class SmallWordFilter : IWordFilter
    {
        public Result<bool> IsPermitted(string word)
        {
            return word.Length > 3;
        }
    }
}