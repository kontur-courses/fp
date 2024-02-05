using TagsCloud.Results;
using TagsCloudVisualization;

namespace TagsCloud.Contracts;

public interface ICloudProcessor
{
    Result<HashSet<WordTagGroup>> SetPositions(HashSet<WordTagGroup> wordGroups);
    Result<HashSet<WordTagGroup>> SetFonts(HashSet<WordTagGroup> wordGroups);
    Result<HashSet<WordTagGroup>> SetColors(HashSet<WordTagGroup> wordGroups);
}