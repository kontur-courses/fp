using System.Drawing;
using TagsCloud.Visualization.WordsReaders;

namespace TagsCloud.Visualization.LayouterCores
{
    public interface ILayouterCore
    {
        Image GenerateImage(IWordsReadService wordsReadService);
    }
}