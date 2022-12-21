using ResultOf;

namespace TagCloud.Common.TagsConverter;

public interface ITagsConverter
{
    Result<IEnumerable<Tag>> ConvertToTags(IEnumerable<string> words, int minFontSize);
}