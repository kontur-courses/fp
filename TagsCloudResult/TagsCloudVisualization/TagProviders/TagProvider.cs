using TagsCloudVisualization.Common;
using TagsCloudVisualization.Extensions;
using TagsCloudVisualization.WordsAnalyzers;
using TagsCloudVisualization.WordsProcessors;
using TextReader = TagsCloudVisualization.TextReaders.TextReader;

namespace TagsCloudVisualization.TagProviders;

public class TagProvider : ITagProvider
{
    private readonly TextReader textReader;
    private readonly IWordsProcessor processor;
    
    public TagProvider(TextReader textReader, IWordsProcessor processor)
    {
        this.textReader = textReader;
        this.processor = processor;
    }
    
    public Result<IEnumerable<Tag>> GetTags()
    {
        return textReader.GetText().Then(x => x.GetAllWords()).Then(words =>
        {
            var wordsWithFreq = new Dictionary<string, int>();

            foreach (var word in processor.Process(words))
                wordsWithFreq[word] = wordsWithFreq.TryGetValue(word, out var value) ? value + 1 : 1;

            var max = wordsWithFreq.Max(x => x.Value);
            return (IEnumerable<Tag>)wordsWithFreq.Select(x => new Tag(x.Key, (double) x.Value / max)).OrderByDescending(x => x.Coeff);
        });
    }
}