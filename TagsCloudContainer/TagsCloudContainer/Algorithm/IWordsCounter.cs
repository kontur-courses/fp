using TagsCloudContainer.Infrastructure;

namespace TagsCloudContainer.Algorithm
{
    public interface IWordsCounter
    {
        public Result<Dictionary<string, int>> CountWords(string pathToSource, string pathToCustomBoringWords);
    }
}
