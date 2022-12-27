using TagsCloudContainer.Core.Results;

namespace TagsCloudContainer.Core.WordsParser.Interfaces
{
    public interface IFileReader
    {
        public Result<IEnumerable<string>> ReadWords();
    }
}
