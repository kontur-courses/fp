using TagsCloud.Visualization.Utils;

namespace TagsCloud.Visualization.WordsReaders
{
    public class SimpleTextReader : IWordsProvider
    {
        private readonly string text;

        public SimpleTextReader(string words) => text = words;

        public Result<string> Read() => text;
    }
}