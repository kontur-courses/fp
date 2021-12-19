namespace TagsCloudContainer.Filters;

public interface IFilter
{
    bool Allows(string word);
}
