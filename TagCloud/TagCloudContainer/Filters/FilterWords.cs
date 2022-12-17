namespace TagCloudContainer.Filters
{

    public class FilterWords : IFilter
    {
        public Result<IEnumerable<string>> Filter(IEnumerable<string> textWords, Func<string, bool> filterFunc)
        {
            return Result.Of(()=> textWords.Select(x => x).Where(filterFunc));
        }
    }
}
