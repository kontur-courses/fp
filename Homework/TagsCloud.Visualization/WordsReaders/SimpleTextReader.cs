using TagsCloud.Visualization.Utils;

namespace TagsCloud.Visualization.WordsReaders
{
    public class SimpleTextReader : IWordsReadService
    {
        private readonly string words;

        public SimpleTextReader(string words) => this.words = words;

        public Result<string> Read() => words;
    }
}