using ResultOf;

namespace TagsCloudContainer.Words
{
    public interface IWordAnalyzer
    {
        Result<WordPack[]> WordPacks { get; }
    }
}