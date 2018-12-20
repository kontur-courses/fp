using ResultOf;

namespace TagsCloudContainer.Words
{
    public interface IWordsReader
    {
        Result<string[]> ReadWords();
    }
}
