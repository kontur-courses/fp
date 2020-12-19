using TagsCloud.ResultPattern;

namespace TagsCloud.FileReader
{
    public interface IWordsReader
    {
        Result<string[]> ReadWords(string path);
    }
}