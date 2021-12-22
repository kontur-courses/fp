using TagsCloud.Visualization.Utils;

namespace TagsCloud.Visualization.TextProviders
{
    public interface ITextProvider
    {
        Result<string> Read();
    }
}