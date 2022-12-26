using TagsCloudContainer.Core.Results;

namespace TagsCloudContainer.Core.WordsParser.Interfaces
{
    public interface IWordsReader
    {
        public Result<IEnumerable<string>> ReadWords();
    }
}
