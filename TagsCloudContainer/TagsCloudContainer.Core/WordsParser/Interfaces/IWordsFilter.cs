using TagsCloudContainer.Core.Results;

namespace TagsCloudContainer.Core.WordsParser.Interfaces
{
    public interface IWordsFilter
    {
        public Result<IEnumerable<string>> RemoveBoringWords(IEnumerable<string> words);
    }
}
