using TagCloud.Utils.ResultPattern;

namespace TagCloud.Utils.Words.Interfaces;

public interface IWordsService
{
    public Result<IEnumerable<string>> GetWords(string path);
}