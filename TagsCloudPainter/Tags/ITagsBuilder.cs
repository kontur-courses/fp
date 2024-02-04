using ResultLibrary;

namespace TagsCloudPainter.Tags;

public interface ITagsBuilder
{
    public Result<List<Tag>> GetTags(List<string> words);
}