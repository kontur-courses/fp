using TagCloudPainter.Common;
using TagCloudPainter.ResultOf;

namespace TagCloudPainter.Builders;

public interface ITagCloudElementsBuilder
{
    Result<IEnumerable<Tag>> GetTags(Dictionary<string, int> dictionary);
}