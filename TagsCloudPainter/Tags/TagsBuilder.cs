using ResultLibrary;
using TagsCloudPainter.Settings.Tag;

namespace TagsCloudPainter.Tags;

public class TagsBuilder : ITagsBuilder
{
    private readonly ITagSettings _settings;

    public TagsBuilder(ITagSettings settings)
    {
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
    }

    public Result<List<Tag>> GetTags(List<string> words)
    {
        var countedWords = CountWords(words);
        var tags = countedWords.Then(countedWords => countedWords
        .Select(wordWithCount => GetTag(wordWithCount.Key, _settings.TagFontSize, wordWithCount.Value, countedWords.Count)
        .GetValueOrThrow())
        .ToList());

        return tags;
    }

    private static Result<Dictionary<string, int>> CountWords(List<string> words)
    {
        var countedWords = Result.Of(() => words
        .GroupBy(word => word)
        .ToDictionary(group => group.Key, group => group.Count()));

        return countedWords;
    }

    private static Result<Tag> GetTag(string tagValue, int fontSize, int tagWordsAmout, int allWordsAmout)
    {
        var tagFontSize = GetTagFontSize(fontSize, tagWordsAmout, allWordsAmout);
        var tag = tagFontSize.Then(tagFontSize => new Tag(tagValue, tagFontSize, tagWordsAmout));

        return tag;
    }

    private static Result<float> GetTagFontSize(int fontSize, int tagCount, int wordsAmount)
    {
        return Result.Of(() => (float)tagCount / wordsAmount * fontSize * 100);
    }
}