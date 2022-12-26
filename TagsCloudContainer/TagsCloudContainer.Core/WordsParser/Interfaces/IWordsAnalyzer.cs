using TagsCloudContainer.Core.Results;

namespace TagsCloudContainer.Core.WordsParser.Interfaces
{
    public interface IWordsAnalyzer
    {
        public Result<Dictionary<string, int>> AnalyzeWords();
    }
}
