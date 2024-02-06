using TagsCloud.Options;
using TagsCloudVisualization;

namespace TagsCloud.Filters;

public interface IFilter
{
    void Apply(HashSet<WordTagGroup> wordGroups, IFilterOptions options);
}