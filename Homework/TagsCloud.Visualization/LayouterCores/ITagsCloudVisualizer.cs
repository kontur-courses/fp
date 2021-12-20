using System.Drawing;
using TagsCloud.Visualization.Utils;
using TagsCloud.Visualization.WordsReaders;

namespace TagsCloud.Visualization.LayouterCores
{
    public interface ITagsCloudVisualizer
    {
        Result<Image> GenerateImage(IWordsProvider wordsProvider);
    }
}