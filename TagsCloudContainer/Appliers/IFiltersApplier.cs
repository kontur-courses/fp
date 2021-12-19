namespace TagsCloudContainer.Appliers;

public interface IFiltersApplier
{
    IEnumerable<string> ApplyFilters(IEnumerable<string> words);
}
