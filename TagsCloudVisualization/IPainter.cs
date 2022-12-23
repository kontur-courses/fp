using System.Collections.Generic;
using ResultOf;
using TagsCloudVisualization.TextFormatters;

namespace TagsCloudVisualization
{
    public interface IPainter
    {
        public Result<None> DrawWordsToFile(List<Word> words, string path);
    }
}