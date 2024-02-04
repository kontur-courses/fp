using TagsCloudVisualization.Common;
using TagsCloudVisualization.Extensions;
using TagsCloudVisualization.TextReaders;
using TagsCloudVisualization.WordsAnalyzers;
using TagsCloudVisualization.WordsProcessors;
using TextReader = TagsCloudVisualization.TextReaders.TextReader;

namespace TagsCloudVisualization.TagProviders;

public class TagProvider : ITagProvider
{
    private readonly ITextReaderFactory textReaderFactory;
    private readonly IWordsProcessor wordsProcessor;
    
    public TagProvider(ITextReaderFactory textReaderFactory, IWordsProcessor wordsProcessor)
    {
        this.textReaderFactory = textReaderFactory;
        this.wordsProcessor = wordsProcessor;
    }
    
    public Result<IEnumerable<Tag>> GetTags()
    {
        return textReaderFactory.GetTextReader()
            .Then(reader => reader.GetText())
            .Then(text => text.GetAllWords())
            .Then(words => wordsProcessor.Process(words))
            .Then(CalculateTags);
    }

    private static IEnumerable<Tag> CalculateTags(IEnumerable<string> words)
    {
        var wordsWithFreq = new Dictionary<string, int>();

        foreach (var word in words)
            wordsWithFreq[word] = wordsWithFreq.TryGetValue(word, out var value) ? value + 1 : 1;

        var max = wordsWithFreq.Max(x => x.Value);
        return wordsWithFreq.Select(x => new Tag(x.Key, (double) x.Value / max)).OrderByDescending(x => x.Coeff);
    }
}