using TagCloudContainer.Result;

namespace TagCloudContainer.Interfaces
{
    public interface IWordFormatter
    {
        Result<IEnumerable<string>> Normalize(IEnumerable<string> textWords, Func<string, string> normalizeFunction);
    }
}
