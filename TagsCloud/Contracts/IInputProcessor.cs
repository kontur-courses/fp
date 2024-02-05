using TagsCloud.Results;
using TagsCloudVisualization;

namespace TagsCloud.Contracts;

public interface IInputProcessor
{
    Result<HashSet<WordTagGroup>> CollectWordGroupsFromFile(string filename);
}