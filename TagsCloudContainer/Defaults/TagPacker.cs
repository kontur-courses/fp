using TagsCloudContainer.Abstractions;

namespace TagsCloudContainer.Defaults;

public class TagPacker : ITagPacker
{
    private readonly ITextAnalyzer textAnalyzer;

    public TagPacker(ITextAnalyzer textAnalyzer)
    {
        this.textAnalyzer = textAnalyzer;
    }

    public IEnumerable<ITag> GetTags()
    {
        var stats = textAnalyzer.AnalyzeText();

        foreach (var (word, count) in stats.Statistics.OrderByDescending(x => x.Value))
        {
            var relativeSize = (double)count / stats.TotalWordCount;

            var tag = new Tag(word, relativeSize);

            yield return tag;
        }
    }
}
