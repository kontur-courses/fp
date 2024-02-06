using TagsCloud.Results;
using TagsCloudVisualization;

namespace TagsCloud.Processors;

public interface IInputProcessor
{
    Result<HashSet<WordTagGroup>> CollectWordGroupsFromFile(string filename);
}