using TagsCloud.Visualization.Models;
using TagsCloud.Visualization.TextProviders;
using TagsCloud.Visualization.Utils;

namespace TagsCloud.Visualization
{
    public interface IWordsService
    {
        Result<Word[]> GetWords(ITextProvider textProvider);
    }
}