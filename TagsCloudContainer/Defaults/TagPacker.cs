using ResultExtensions;
using ResultOf;
using TagsCloudContainer.Abstractions;

namespace TagsCloudContainer.Defaults;

public class TagPacker : ITagPacker
{
    private readonly ITextAnalyzer textAnalyzer;

    public TagPacker(ITextAnalyzer textAnalyzer)
    {
        this.textAnalyzer = textAnalyzer;
    }

    public Result<IEnumerable<ITag>> GetTags()
    {
        return textAnalyzer.AnalyzeText().Then(PackTags);
    }

    private IEnumerable<ITag> PackTags(ITextStats stats)
    {
        foreach (var (word, count) in stats.Statistics.OrderByDescending(x => x.Value))
        {
            var relativeSize = (double)count / stats.TotalWordCount;

            var tag = new Tag(word, relativeSize);

            yield return tag;
        }
    }
}
