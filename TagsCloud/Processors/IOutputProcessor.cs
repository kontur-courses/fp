using TagsCloud.Results;
using TagsCloudVisualization;

namespace TagsCloud.Processors;

public interface IOutputProcessor
{
    Result<HashSet<WordTagGroup>> SaveVisualization(HashSet<WordTagGroup> wordGroups, string filename);
}