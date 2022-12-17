namespace TagCloudContainer.Filters
{
    public interface IFilter
    {
        Result<IEnumerable<string>> Filter(IEnumerable<string> textWords, Func<string, bool> boolFunc);
    }
}
