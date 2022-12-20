using System.Drawing;
using TagsCloudVisualization.Parsing;
using TagsCloudVisualization.Reading;

namespace TagsCloudVisualization.Words;

public class CustomWordsLoader : IWordsLoader
{
    private readonly ITextReader _textReader;
    private readonly ITextParser _textParser;
    private readonly IWordsFilter _wordsFilter;
    private readonly IWordsSizeCalculator _wordsSizeCalculator;

    public CustomWordsLoader(ITextReader textReader, ITextParser textParser, IWordsSizeCalculator wordsSizeCalculator, IWordsFilter wordsFilter)
    {
        _textReader = textReader;
        _textParser = textParser;
        _wordsSizeCalculator = wordsSizeCalculator;
        _wordsFilter = wordsFilter;
    }

    public Result<IEnumerable<Word>> LoadWords(VisualizationOptions options)
    {
        var rawWordsResult = _textReader.ReadText()
            .Then(text => _textParser.ParseWords(text))
            .RefineError("Cant parse text");

        if (!rawWordsResult.IsSuccess)
            return Result.Fail<IEnumerable<Word>>(rawWordsResult.Error);

        var rawWords = rawWordsResult.GetValueOrThrow();

        var wordsAndCount = rawWords.GroupBy(r => r.ToLowerInvariant())
            .ToDictionary(r => r.Key, r => r.Count());

        var filteredWordsResult = _wordsFilter.FilterWords(wordsAndCount, options);
        if (!filteredWordsResult.IsSuccess)
            return Result.Fail<IEnumerable<Word>>(filteredWordsResult.Error);

        var filteredWords = filteredWordsResult.GetValueOrThrow();
        var takeWords = filteredWords;

        if (options.TakeMostPopularWords > 0)
            takeWords = filteredWords.OrderByDescending(r => r.Value)
                .Take(options.TakeMostPopularWords)
                .ToDictionary(r => r.Key, r => r.Value);

        var sizesForWordsResult = _wordsSizeCalculator.CalcSizeForWords(takeWords, options.MinFontSize, options.MaxFontSize);
        if (!sizesForWordsResult.IsSuccess)
            return Result.Fail<IEnumerable<Word>>(sizesForWordsResult.Error);

        var sizesForWords = sizesForWordsResult.GetValueOrThrow();

        var words = new List<Word>();
        foreach (var group in takeWords)
        {
            if (!sizesForWords.TryGetValue(group.Key, out var wordSize))
                wordSize = options.MinFontSize;

            var word = new Word(group.Key, group.Value, wordSize);
            words.Add(word);
        }

        return words.OrderByDescending(r => r.Size).ToList();
    }
}