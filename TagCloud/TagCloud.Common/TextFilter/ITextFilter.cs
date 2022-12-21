using ResultOf;

namespace TagCloud.Common.TextFilter;

public interface ITextFilter
{
    public Result<List<string>> FilterAllWords(IEnumerable<string> lines, int boringWordsLength);
}