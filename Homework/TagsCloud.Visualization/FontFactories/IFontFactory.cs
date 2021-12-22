using TagsCloud.Visualization.Models;

namespace TagsCloud.Visualization.FontFactories
{
    public interface IFontFactory
    {
        FontDecorator GetFont(Word word, int minCount, int maxCount);
    }
}